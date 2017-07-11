using CraigslistMapsGoogleAPIParser.Components;
using CraigslistMapsGoogleAPIParser.DataProviders;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace CraigslistMapsGoogleAPIParser
{
    public class AddressParser
    {
        WebProxy proxy;
        public AddressParser()
        {
           //proxy = ProxySolver.Instance.getNewProxy();
        }

        private WebProxy UpdateInternalProxy()
        {
            return proxy = ProxySolver.Instance.getNewProxy();
        }
        #region Методы для получения данных местоположения через Google Maps Geocoding API
        /// <summary>
        /// Получает результат GET-запроса в виде JSON от Google Maps Geocoding 
        /// </summary>
        /// <param name="addressOrLatlng">адрес или координаты через запятую. Пример: "29.739600,-95.446000"</param>
        /// <param name="type">Тип геокодирования(по адресу или по координатам)</param>
        /// <param name="apikey">Ключ для работы с API от Google, выделенный под конкретный проект</param>
        /// <returns>Возвращает массив объектов c результатами мест в JSON </returns>
        public static string GetJsonMapResponse(string addressOrLatlng, Constants.TypeOfMapGrabbing type, string apiKey = "")
        {
            string GETRequestLink = type == Constants.TypeOfMapGrabbing.ByAddress ? Constants.GoogleRequestParams.AddressMapsQueryPattern + addressOrLatlng.Replace(" ", "+") :
                Constants.GoogleRequestParams.LatlngMapsQueryPattern + addressOrLatlng; //в адресе заменяем пробелы на плюсы для соответствия формату запроса + исходя из типа, задаем правильный текст запроса
            if (apiKey != "")
            {
                GETRequestLink += "&key=" + apiKey;
            }
            return WebHelpers.GetWebResponceContent(GETRequestLink);
        }

        public static string GetJsonMapResponseThrowProxy(string addressOrLatlng, Constants.TypeOfMapGrabbing type, string apiKey = "")
        {
            string GETRequestLink = type == Constants.TypeOfMapGrabbing.ByAddress ? Constants.GoogleRequestParams.AddressMapsQueryPattern + addressOrLatlng.Replace(" ", "+") :
                Constants.GoogleRequestParams.LatlngMapsQueryPattern + addressOrLatlng; //в адресе заменяем пробелы на плюсы для соответствия формату запроса + исходя из типа, задаем правильный текст запроса
            if (apiKey != "")
            {
                GETRequestLink += "&key=" + apiKey;
            }
            string JsonResponse;
            while (true) //получаем корректный ответ до победного конца
            {

                JsonResponse = WebHelpers.GetWebResponceContentThrowProxy(GETRequestLink.Replace("+%C3%A2%C2%80%C2%A2", ""), ProxySolver.Instance.getNewProxy());
                GeocodeJsonObject o = ReadJsonToObject(JsonResponse);
                if(o != null)
                {
                    if (o.status != Constants.GoogleMapsGeocodingStatusCodes.OVER_QUERY_LIMIT)
                    {
                        break;
                    }
                }
                          
            }
            return JsonResponse;
        }

        #endregion

        #region Методы для обработки исходной таблицы Offers БД CraigsList

        public List<AddressInfo> GetAddressesByRange(long start, long end)
        {
            DataTable CraigsListAddressesDataTable = DataProvider.Instance.GetDataset(start, end).Tables[0];
            //DataProvider.ConsoleView(CraigsListAddressesDataTable); //выводим на консоль
            ParallelOptions options = new ParallelOptions();
            options.MaxDegreeOfParallelism = Convert.ToInt32(Resources.MaxDegreeOfParallelism);

            List<AddressInfo> addresses = new List<AddressInfo>();
            //Parallel.For(0, CraigsListAddressesDataTable.Rows.Count, options, (cur) =>
            for(int i = 0; i < CraigsListAddressesDataTable.Rows.Count; i++)
            {
                addresses.Add(new AddressInfo(CraigsListAddressesDataTable.Rows[i]));
            }
            return addresses;
        }
        #endregion


        public void StartGrabbing()
        {
            ParallelOptions options = new ParallelOptions();
            options.MaxDegreeOfParallelism = Convert.ToInt32(Resources.MaxDegreeOfParallelism);
            long CraigslistCount = DataProviders.DataProvider.Instance.GetCount(); //смотрим, сколько записей в таблице
            Console.WriteLine(CraigslistCount);
            int range = 1000;
            int startPosition = 1;
            try
            {
                string p = File.ReadAllText("position.txt");
                startPosition = int.Parse(p);
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
                startPosition = 450001;
            }
            for (int i = startPosition; i <= CraigslistCount; i += range)
            {
                if ((i) % 1000 == 0)
                {
                    Console.WriteLine("Now {0} cells", i);
                    string pos = i.ToString();
                    File.WriteAllText("position.txt", pos);
                }
                List<AddressInfo> adresses;
                if (i + range > CraigslistCount)
                {
                    adresses = GetAddressesByRange(i, CraigslistCount);
                }
                else
                {
                    adresses = GetAddressesByRange(i, i + range - 1);
                }
                Parallel.ForEach(adresses, options, address => {
                    
                        //Console.WriteLine(adresses[j].PlaceLink);
                        address.ParseJSON();
                    if(address.JSON != String.Empty && address.JSON != Constants.WebAttrsNames.NotFound)
                    {
                        if (AddressParser.ReadJsonToObject(address.JSON).status == Constants.GoogleMapsGeocodingStatusCodes.OVER_QUERY_LIMIT)
                        {
                            return;
                        }
                    }
                        address.InsertIntoDb();
                });
                //Parallel.ForEach(adresses,options, address => {
                //    address.ParseJSON();
                //    address.InsertIntoDb();
                //});

            }           

        }

        public List<AddressInfo> GetJSONPlacesFromDb(long start, long end)
        {
            DataTable CraigsListAddressesDataTable = DataProvider.Instance.GetDatasetFromCraigslistPlaces(start, end).Tables[0];
            //DataProvider.ConsoleView(CraigsListAddressesDataTable); //выводим на консоль
            ParallelOptions options = new ParallelOptions();
            options.MaxDegreeOfParallelism = Convert.ToInt32(Resources.MaxDegreeOfParallelism);

            List<AddressInfo> addresses = new List<AddressInfo>();
            //Parallel.For(0, CraigsListAddressesDataTable.Rows.Count, options, (cur) =>
            for (int i = 0; i < CraigsListAddressesDataTable.Rows.Count; i++)
            {
                addresses.Add(new AddressInfo(CraigsListAddressesDataTable.Rows[i]));
            }
            return addresses;
        }

        public static GeocodeJsonObject ReadJsonToObject(string json)
        {
            GeocodeJsonObject deserializedObject = new GeocodeJsonObject();
            //MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(json));
            //DataContractJsonSerializer ser = new DataContractJsonSerializer(deserializedUser.GetType());
            //deserializedUser = ser.ReadObject(ms) as User;
            //ms.Close();
            try
            {
                deserializedObject = JsonConvert.DeserializeObject<GeocodeJsonObject>(json);
            }
            catch
            {
                deserializedObject = null;
            }
            
            return deserializedObject;
        }
        /// <summary>
        /// Берет строки из таблицы, парсит JSON и раскидывает по столбцам 
        /// </summary>
        public void ConvertJsonStringFromDbToCells()
        {
            long CraigslistCount = DataProviders.DataProvider.Instance.GetPlacesCount(); //смотрим, сколько записей в таблице
            Console.WriteLine(CraigslistCount);
            int range = 10000;
            for (int i = 638000; i <= CraigslistCount; i += range)
            {
                if ((i - 1) % 2000 == 0)
                {
                    Console.WriteLine("Now {0} cells", i - 1);
                }
                List<AddressInfo> adresses;
                if (i + range > CraigslistCount)
                {
                    adresses = GetJSONPlacesFromDb(i, CraigslistCount);
                }
                else
                {
                    adresses = GetJSONPlacesFromDb(i, i + range - 1);
                }
                //for (int j = 0; j < adresses.Count; j++)
                //{
                ParallelOptions options = new ParallelOptions();
                options.MaxDegreeOfParallelism = Convert.ToInt32(Resources.MaxDegreeOfParallelism);
                Parallel.ForEach(adresses, options, address =>
                {
                    //Console.WriteLine(adresses[j].PlaceLink);
                    if (address.PlaceLink != String.Empty && address.PlaceLink != Constants.WebAttrsNames.NotFound)
                    {
                        GeocodeJsonObject o = ReadJsonToObject(address.PlaceLink);
                        RooftopResultPlace res = new RooftopResultPlace(o, address.ID);

                        res.InsertToDb();
                    }
                    else
                    {
                        Console.WriteLine("Запись с номером ID={0} не содержит JSON", address.ID);
                    }
                });
                

            }
        }

    }
}

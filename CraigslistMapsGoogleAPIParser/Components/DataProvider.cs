﻿using System.Data.SqlClient;
using System.Data;
using System;
using CraigslistMapsGoogleAPIParser.Components;

namespace CraigslistMapsGoogleAPIParser.DataProviders
{
    public class DataProvider : SingleTone<DataProvider>
    {

        /// <summary>
        /// returns default Connection
        /// </summary>
        public SqlConnection Connection
        {

            get
            {

                SqlConnection sqlConnection = null;
                if (sqlConnection == null)
                {
                    sqlConnection = new SqlConnection(Resources.DbCraigslistConnectionString);
                }
                if (string.IsNullOrEmpty(sqlConnection.ConnectionString))
                {
                    sqlConnection.ConnectionString = Resources.DbCraigslistConnectionString;
                }
                return sqlConnection;
            }
        }

        public void ExecureSP(SqlCommand sqlCommand)
        {
            bool needCloseConnection = true;
            int numberOfRowsAffected = 0;
            if (sqlCommand.CommandType != CommandType.StoredProcedure)
            {
                throw new Exception("Not StoredProcedure");
            }
            try
            {
                //If connection is already opened it means that it is a transaction and we must not close 
                //connection after this command execution, because next command in this transaction uses 
                //the same connection.
                if (sqlCommand.Connection.State != ConnectionState.Open)
                {
                    sqlCommand.Connection.Open();
                }
                else
                {
                    needCloseConnection = false;
                }

                numberOfRowsAffected = sqlCommand.ExecuteNonQuery();//return the number of rows affected
                //TODO: check numberOfRowsAffected?
            }
            catch (SqlException ex)
            {
                //throw new Exception(ex.Message, ex);
                Console.WriteLine(ex.Message);
            }
            finally
            {
                if (needCloseConnection)
                {
                    sqlCommand.Connection.Close();
                }
            }
        }








        #region Public Methods

        /// <summary>
        /// Create SQL command for stored procedure
        /// </summary>    
        /// <param name="spName">name of the stored procedure</param>
        /// <returns>SQL command</returns>
        /// <remarks></remarks>
        public SqlCommand CreateSQLCommandForSP(string storedProcedureName, string SqlConnectionString)
        {
            SqlCommand command = new SqlCommand(storedProcedureName, new SqlConnection(SqlConnectionString));
            command.CommandType = CommandType.StoredProcedure;
            // command.Connection.Open();
            return command;
        }

        /// <summary>
        /// Create SQL command for string query
        /// </summary>    
        /// <param name="spName">name of the stored procedure</param>
        /// <returns>SQL command</returns>
        /// <remarks></remarks>
        public SqlCommand CreateSQLCommand(string query, string SqlConnectionString)
        {
            SqlCommand command = new SqlCommand(query, new SqlConnection(SqlConnectionString));
            command.CommandType = CommandType.Text;
            return command;
        }


        /// <summary>
        /// Create input SQL parametet, its name is @ and column name
        /// </summary>
        /// <param name="columnName">Column name which matches with parameter</param>
        /// <param name="dbType">Parameter type</param>
        /// <param name="value">Parameter value</param>
        /// <returns>Filled SQL parameter</returns>
        /// <remarks></remarks>
        public SqlParameter CreateSqlParameter(string columnName, SqlDbType dbType, object value)
        {
            return CreateSqlParameter(columnName, dbType, value, ParameterDirection.Input);
        }

        /// <summary>
        /// Create SQL parametet, its name is @ and column name
        /// </summary>
        /// <param name="columnName">Column name which matches with parameter</param>
        /// <param name="dbType">Parameter type</param>
        /// <param name="value">Parameter value</param>
        /// <param name="direction">Parameter direction</param>
        /// <returns>Filled SQL parameter</returns>
        /// <remarks></remarks>
        public SqlParameter CreateSqlParameter(string columnName, SqlDbType dbType, object value, ParameterDirection direction)
        {
            // Add parametors
            SqlParameter param = new SqlParameter(string.Format("@{0}", columnName), dbType);

            param.Direction = direction;
            param.Value = value;

            return param;
        }

        /// <summary>
        /// Makes parameterName satisfying t-sql syntax (parameterName - > @parameterName)
        /// </summary>
        /// <param name="parameterName"></param>
        /// <returns></returns>
        public string SqlParameterName(string parameterName)
        {
            return string.Format("@{0}", parameterName);
        }

        internal DataSet GetDatasetFromDb(string query,string SQLConnectionString)
        {
            SqlDataAdapter da = new SqlDataAdapter();
            SqlCommand cmd = CreateSQLCommand(query,SQLConnectionString);
            da.SelectCommand = cmd;
            DataSet ds = new DataSet();
            da.Fill(ds);
            return ds;
        }

        internal DataSet GetDatasetFromDbByStoredProcedure(SqlCommand SP_Name)
        {
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = SP_Name;
            DataSet ds = new DataSet();
            da.Fill(ds);
            return ds;
        }

        /// <summary>
        /// Console Output for datatable
        /// </summary>
        /// <param name="table">Table from Dataset</param>
        internal static void ConsoleView(DataTable table)
        {
            Console.WriteLine("--- ConsoleTable(" + table.TableName + ") ---");
            int zeilen = table.Rows.Count;
            int spalten = table.Columns.Count;

            // Header
            for (int i = 0; i < table.Columns.Count; i++)
            {
                string s = table.Columns[i].ToString();
                Console.Write(String.Format("{0,-40} | ", s));
            }
            Console.WriteLine();
            for (int i = 0; i < table.Columns.Count; i++)
            {
                Console.Write("-----------------------------------------|-");
            }
            Console.WriteLine();

            Console.WriteLine();
            // Data
            for (int i = 0; i < zeilen; i++)
            {
                DataRow row = table.Rows[i];
                //Console.WriteLine("{0} {1} ", row[0], row[1]);
                for (int j = 0; j < spalten; j++)
                {
                    string s = row[j].ToString();
                    s = s.Replace("\n", " ");
                    if (s.Length > 40) s = s.Substring(0, 37) + "...";
                    Console.Write(String.Format("{0,-40} | ", s));
                }
                Console.WriteLine();
            }
            for (int i = 0; i < table.Columns.Count; i++)
            {
                Console.Write("-----------------------------------------|-");
            }
            Console.WriteLine();
        }

        //internal List<Region> GetRegionsFromDb()
        //{
        //    SqlDataAdapter da = new SqlDataAdapter();
        //    SqlCommand cmd = CreateSQLCommand(Resources.QuerySelectRegionsUndone);
        //    da.SelectCommand = cmd;
        //    DataSet ds = new DataSet();
        //    da.Fill(ds);
        //    List<Region> regions = new List<Region>();
        //    foreach (DataRow row in ds.Tables[0].Rows)
        //    {
        //        regions.Add(new Region(row));
        //    }            
        //    return regions;
        //}

        //internal void FinalizeRegion(Region r)
        //{
        //    try
        //    {
        //        SqlCommand finalize = Instance.CreateSQLCommandForSP(Resources.SP_FinalizeRegion);
        //        finalize.Parameters.AddWithValue("@ID", r.ID);
        //        Instance.ExecureSP(finalize);
        //    }
        //    catch(Exception ex)
        //    {

        //        Console.WriteLine("Ошибка при процедуре финализировании регионаб {0}", ex.Message);
        //        throw new Exception(ex.Message);
        //    }
        //}

        //internal void InsertOfferToDb(Offer o)
        //{
        //try
        //{
        //SqlCommand insertOffer = Instance.CreateSQLCommandForSP(Resources.SP_InsertOffer);
        //insertOffer.Parameters.AddWithValue(Constants.OfferCellNames.postId, o.postId);
        //insertOffer.Parameters.AddWithValue(Constants.OfferCellNames.agentName,             ((object)o.agentName                 )??(DBNull.Value));
        //insertOffer.Parameters.AddWithValue(Constants.OfferCellNames.addressForDisplay,     ((object)o.addressForDisplay         )??(DBNull.Value));
        //insertOffer.Parameters.AddWithValue(Constants.OfferCellNames.city,                  o.city                  );
        //insertOffer.Parameters.AddWithValue(Constants.OfferCellNames.county,               o.county               );
        //insertOffer.Parameters.AddWithValue(Constants.OfferCellNames.countyFIPS,            ((object)o.countyFIPS                )??(DBNull.Value));
        //insertOffer.Parameters.AddWithValue(Constants.OfferCellNames.dataPhotos,            ((object)o.dataPhotos                )??(DBNull.Value));
        //insertOffer.Parameters.AddWithValue(Constants.OfferCellNames.feedId,                ((object)o.feedId                    )??(DBNull.Value));
        //insertOffer.Parameters.AddWithValue(Constants.OfferCellNames.formattedBedAndBath,   ((object)o.formattedBedAndBath       )??(DBNull.Value));
        //insertOffer.Parameters.AddWithValue(Constants.OfferCellNames.formattedPrice,        ((object)o.formattedPrice            )??(DBNull.Value));
        //insertOffer.Parameters.AddWithValue(Constants.OfferCellNames.formattedSqft,         ((object)o.formattedSqft             )??(DBNull.Value));
        //insertOffer.Parameters.AddWithValue(Constants.OfferCellNames.hasPhotos,             ((object)o.hasPhotos                 )??(DBNull.Value));
        //insertOffer.Parameters.AddWithValue(Constants.OfferCellNames.isRentalCommunity,     ((object)o.isRentalCommunity         )??(DBNull.Value));
        //insertOffer.Parameters.AddWithValue(Constants.OfferCellNames.latitude,              ((object)o.latitude                  )??(DBNull.Value));
        //insertOffer.Parameters.AddWithValue(Constants.OfferCellNames.longitude,             ((object)o.longitude                 )??(DBNull.Value));
        //insertOffer.Parameters.AddWithValue(Constants.OfferCellNames.locationId,            ((object)o.locationId                )??(DBNull.Value));
        //insertOffer.Parameters.AddWithValue(Constants.OfferCellNames.listingId,             ((object)o.listingId                 )??(DBNull.Value));
        //insertOffer.Parameters.AddWithValue(Constants.OfferCellNames.numBathrooms,          ((object)o.numBathrooms              )??(DBNull.Value));
        //insertOffer.Parameters.AddWithValue(Constants.OfferCellNames.numBedrooms,           ((object)o.numBedrooms               )??(DBNull.Value));
        //insertOffer.Parameters.AddWithValue(Constants.OfferCellNames.numBeds,               ((object)o.numBeds                   )??(DBNull.Value));
        //insertOffer.Parameters.AddWithValue(Constants.OfferCellNames.numFullBathrooms,      ((object)o.numFullBathrooms          )??(DBNull.Value));
        //insertOffer.Parameters.AddWithValue(Constants.OfferCellNames.numPartialBathrooms,   ((object)o.numPartialBathrooms       )??(DBNull.Value));
        //insertOffer.Parameters.AddWithValue(Constants.OfferCellNames.price,                 ((object)o.price                     )??(DBNull.Value));
        //insertOffer.Parameters.AddWithValue(Constants.OfferCellNames.FTRank,            ((object)o.FTRank                )??(DBNull.Value));
        //insertOffer.Parameters.AddWithValue(Constants.OfferCellNames.rentalType,            ((object)o.rentalType                )??(DBNull.Value));
        //insertOffer.Parameters.AddWithValue(Constants.OfferCellNames.zipCode,               ((object)o.zipCode                   )??(DBNull.Value));
        //insertOffer.Parameters.AddWithValue(Constants.OfferCellNames.streetNumber,          ((object)o.streetNumber              )??(DBNull.Value));
        //insertOffer.Parameters.AddWithValue(Constants.OfferCellNames.thumbnail,             ((object)o.thumbnail                 )??(DBNull.Value));
        //insertOffer.Parameters.AddWithValue(Constants.OfferCellNames.sqft,                  ((object)o.sqft                      )??(DBNull.Value));
        //insertOffer.Parameters.AddWithValue(Constants.OfferCellNames.stateCode,                      o.stateCode             );
        //insertOffer.Parameters.AddWithValue(Constants.OfferCellNames.stateName,                      o.stateName         );
        //insertOffer.Parameters.AddWithValue(Constants.OfferCellNames.street,                ((object)o.street                    )??(DBNull.Value));
        //insertOffer.Parameters.AddWithValue(Constants.OfferCellNames.phone,                 ((object)o.phone                     )??(DBNull.Value));
        //insertOffer.Parameters.AddWithValue(Constants.OfferCellNames.idealIncome,           ((object)o.idealIncome               )??(DBNull.Value));
        //insertOffer.Parameters.AddWithValue(Constants.OfferCellNames.metaInfo,              ((object)o.metaInfo                  )??(DBNull.Value));
        //insertOffer.Parameters.AddWithValue(Constants.OfferCellNames.features,              ((object)o.features                  )??(DBNull.Value));
        //insertOffer.Parameters.AddWithValue(Constants.OfferCellNames.communityOtherFeatures,((object)o.communityOtherFeatures    )??(DBNull.Value));
        //insertOffer.Parameters.AddWithValue(Constants.OfferCellNames.communityFloors,       ((object)o.communityFloors           )??(DBNull.Value));
        //insertOffer.Parameters.AddWithValue(Constants.OfferCellNames.directLink,           o.directLink           );







        //Instance.ExecureSP(insertOffer);
        //}
        //catch (Exception ex)
        //{

        //Console.WriteLine("Ошибка при процедуре финализировании регионаб {0}", ex.Message);
        //throw new Exception(ex.Message);
        //}
        //}

        #endregion

        internal void InsertState(string StateName, string Link)
        {
            try
            {
                SqlCommand insertState = Instance.CreateSQLCommandForSP(Resources.SP_InsertState,"");
                
                insertState.Parameters.AddWithValue("@StateName", StateName);
                insertState.Parameters.AddWithValue("@Link", Link);
                Instance.ExecureSP(insertState);
            }
            catch(Exception ex)
            {
                Console.WriteLine("Всатвка записи о штате прошла неудачно!" + ex.Message);
                Console.ReadLine();
            }
        }
        /// <summary>
        /// Получает Записи из БД Craigslist 
        /// </summary>
        /// <param name="begin">Параметр начального индекса записсей в БД включительно</param>
        /// <param name="end">Параметр конечного индекса записсей в БД включительно</param>
        /// <returns>Dataset c записями согласно процедуре</returns>
        public DataSet GetDataset(long begin, long end)
        {
            SqlConnection conn = new SqlConnection(Resources.DbCraigslistConnectionString);
            SqlDataAdapter da = new SqlDataAdapter();
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = $"EXEC [dbo].[{Resources.SP_GetAddressRange}] @BEGIN = {begin}, @END = {end}";
            da.SelectCommand = cmd;
            DataSet ds = new DataSet();

            conn.Open();
            da.Fill(ds);
            conn.Close();

            return ds;
        }
        /// <summary>
        /// Получает Записи из БД CraigslistPlaces 
        /// </summary>
        /// <param name="begin">Параметр начального индекса записсей в БД включительно</param>
        /// <param name="end">Параметр конечного индекса записсей в БД включительно</param>
        /// <returns>Dataset c записями о местоположении</returns>
        public DataSet GetDatasetFromCraigslistPlaces(long begin, long end)
        {
            SqlConnection conn = new SqlConnection(Resources.DbCraigslistPlacesConnectionString);
            SqlDataAdapter da = new SqlDataAdapter();
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = $"EXEC [dbo].[{Resources.SP_GetPlacesRange}] @Start = {begin}, @End = {end}";
            da.SelectCommand = cmd;
            DataSet ds = new DataSet();

            conn.Open();
            da.Fill(ds);
            conn.Close();

            return ds;
        }



        internal long GetCount()
        {
            SqlConnection conn = new SqlConnection(Resources.DbCraigslistConnectionString);
            SqlDataAdapter da = new SqlDataAdapter();
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = $"SELECT MAX(ID) FROM Offerstable";
            conn.Open();

            SqlDataReader rd = cmd.ExecuteReader();
            long colsCount = -1;
            if (rd.HasRows)
            {
                rd.Read(); // read first row                
                colsCount = rd.GetInt64(0);
            }
            conn.Close();
            return colsCount;
        }
        internal long GetPlacesCount()
        {
            SqlConnection conn = new SqlConnection(Resources.DbCraigslistPlacesConnectionString);
            SqlDataAdapter da = new SqlDataAdapter();
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = $"SELECT MAX(ID) FROM ParsedPlaces";
            conn.Open();

            SqlDataReader rd = cmd.ExecuteReader();
            long colsCount = -1;
            if (rd.HasRows)
            {
                rd.Read(); // read first row                
                colsCount = rd.GetInt64(0);
            }
            conn.Close();
            return colsCount;
        }
    }
}
﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан программой.
//     Исполняемая версия:4.0.30319.42000
//
//     Изменения в этом файле могут привести к неправильной работе и будут потеряны в случае
//     повторной генерации кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CraigslistMapsGoogleAPIParser {
    using System;
    
    
    /// <summary>
    ///   Класс ресурса со строгой типизацией для поиска локализованных строк и т.д.
    /// </summary>
    // Этот класс создан автоматически классом StronglyTypedResourceBuilder
    // с помощью такого средства, как ResGen или Visual Studio.
    // Чтобы добавить или удалить член, измените файл .ResX и снова запустите ResGen
    // с параметром /str или перестройте свой проект VS.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        /// <summary>
        ///   Возвращает кэшированный экземпляр ResourceManager, использованный этим классом.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("CraigslistMapsGoogleAPIParser.Resources", typeof(Resources).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Перезаписывает свойство CurrentUICulture текущего потока для всех
        ///   обращений к ресурсу с помощью этого класса ресурса со строгой типизацией.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на Data Source=rentml.database.windows.net;Initial Catalog=CraigsList;Integrated Security=False;User ID=rentml;Password=mlrent-1404;Connect Timeout=15;Encrypt=False;TrustServerCertificate=True.
        /// </summary>
        public static string DbCraigslistConnectionString {
            get {
                return ResourceManager.GetString("DbCraigslistConnectionString", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на Data Source=NOTEBOOK;Integrated Security=True;Initial Catalog=CraigslistPlaces.
        /// </summary>
        public static string DbCraigslistPlacesConnectionString {
            get {
                return ResourceManager.GetString("DbCraigslistPlacesConnectionString", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на AIzaSyArN6iEXHOZRBjvfrnMV3VcqKUvzP7Rm1o.
        /// </summary>
        public static string GoogleAPIKey {
            get {
                return ResourceManager.GetString("GoogleAPIKey", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на 2.
        /// </summary>
        public static string MaxDegreeOfParallelism {
            get {
                return ResourceManager.GetString("MaxDegreeOfParallelism", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на 400.
        /// </summary>
        public static string MaxProxyPing {
            get {
                return ResourceManager.GetString("MaxProxyPing", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на 3.
        /// </summary>
        public static string NumberOfLoadRetrying {
            get {
                return ResourceManager.GetString("NumberOfLoadRetrying", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на proxylist.txt.
        /// </summary>
        public static string ProxyList {
            get {
                return ResourceManager.GetString("ProxyList", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на SP_getAddressListByRange.
        /// </summary>
        public static string SP_GetAddressRange {
            get {
                return ResourceManager.GetString("SP_GetAddressRange", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на SP_GetPlacesRange.
        /// </summary>
        public static string SP_GetPlacesRange {
            get {
                return ResourceManager.GetString("SP_GetPlacesRange", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на InsertPlaceInfo.
        /// </summary>
        public static string SP_InsertPlaceInfo {
            get {
                return ResourceManager.GetString("SP_InsertPlaceInfo", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на InsertState.
        /// </summary>
        public static string SP_InsertState {
            get {
                return ResourceManager.GetString("SP_InsertState", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на SP_SetParsedJsonInfo.
        /// </summary>
        public static string SP_SetParsedJsonInfo {
            get {
                return ResourceManager.GetString("SP_SetParsedJsonInfo", resourceCulture);
            }
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading;
using System.Xml.Serialization;
using System.Globalization;


namespace Logitude.AmitalMessaging.Utils
{
    internal class CachingXmlSerializerFactory
    {
        private static readonly Dictionary<string, XmlSerializer> _Cache = new Dictionary<string, XmlSerializer>();

        private static readonly object _SyncRoot = new object();

        public static XmlSerializer Create(Type type)
        {
            return new XmlSerializer(type);
        }
        public static XmlSerializer Create(Type type, string defaultNamespace)
        {
            if (type == null) throw new ArgumentNullException("type");
            if (String.IsNullOrWhiteSpace(defaultNamespace))
            {
                return Create(type);
            }

            var key = String.Format(CultureInfo.InvariantCulture, "t{0}:N{1}", type, defaultNamespace);

            lock (_SyncRoot)
            {
                if (!_Cache.ContainsKey(key))
                {
                    _Cache.Add(key, new XmlSerializer(type, defaultNamespace));
                }
            }

            return _Cache[key];
        }

        public static XmlSerializer Create(Type type, XmlRootAttribute root)
        {
            if (type == null) throw new ArgumentNullException("type");
            if (root == null)
            {
                return Create(type);
                //throw new ArgumentNullException("root");
            }

            var key = String.Format(CultureInfo.InvariantCulture, "t{0}:r{1}", type, root.ElementName);

            lock (_SyncRoot)
            {
                if (!_Cache.ContainsKey(key))
                {
                    _Cache.Add(key, new XmlSerializer(type, root));
                }
            }

            return _Cache[key];
        }
        public static XmlSerializer Create(Type type, XmlAttributeOverrides overrides)
        {
            if (type == null) throw new ArgumentNullException("type");
            if (overrides == null)
            {
                return Create(type);
                throw new ArgumentNullException("overrides");
            }


            var key = String.Format(CultureInfo.InvariantCulture, "t{0}:overrides{1}", type, overrides[type].XmlText);

            lock (_SyncRoot)
            {
                if (!_Cache.ContainsKey(key))
                {
                    _Cache.Add(key, new XmlSerializer(type, overrides));
                }
            }

            return _Cache[key];

        }
        public static XmlSerializer Create(Type type, XmlRootAttribute root, string defaultNamespace)
        {
            if (type == null) throw new ArgumentNullException("type");

            if (root == null)
            {
                return Create(type, defaultNamespace);

            }
            if (String.IsNullOrWhiteSpace(defaultNamespace))
            {
                return Create(type, root);
            }

            var key = String.Format(CultureInfo.InvariantCulture, "t{0}:r{1}:n{2}", type, root.ElementName, defaultNamespace);

            lock (_SyncRoot)
            {
                if (!_Cache.ContainsKey(key))
                {
                    _Cache.Add(key, new XmlSerializer(type, null, null, root, defaultNamespace));
                }
            }

            return _Cache[key];
        }
        public static XmlSerializer Create<T>(XmlRootAttribute root)
        {
            return Create(typeof(T), root);
        }
        public static XmlSerializer Create<T>()
        {
            return Create(typeof(T));
        }
        public static XmlSerializer Create<T>(string defaultNamespace)
        {
            return Create(typeof(T), defaultNamespace);
        }
        public static XmlSerializer Create<T>(XmlRootAttribute root, string defaultNamespace)
        {
            return Create(typeof(T), root, defaultNamespace);
        }


    }
}
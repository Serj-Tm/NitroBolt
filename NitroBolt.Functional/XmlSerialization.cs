using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.IO.Compression;
using System.Xml;
using System.Xml.Serialization;
using System.Linq;

namespace NitroBolt.Functional
{
  public class XmlSerialization
  {
    #region Saving

    /// <summary>
    /// For Safe Serialization
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="fileName"></param>
    public static void Save(object obj, string fileName)
    {
      using (var ms = new MemoryStream())
      {
        Save(obj, ms);
        using (FileStream fs = new FileStream(fileName, FileMode.Create, FileAccess.Write))
          ms.WriteTo(fs);
      }
    }

    public static void QuickSave(object obj, string fileName)
    {
      using (FileStream file = new FileStream(fileName, FileMode.Create))
      {
        Save(obj, file);
      }
    }

    public static void SaveToZip(object obj, string fileName)
    {
      using (MemoryStream ms = new MemoryStream())
      {
        SaveToZip(obj, ms);
        using (FileStream fs = new FileStream(fileName, FileMode.Create, FileAccess.Write))
        {
          //ms.WriteTo(fs); //ms то уже закрыт
          byte[] buffer = ms.ToArray();
          fs.Write(buffer, 0, buffer.Length);
        }
      }
    }

    public static void SaveToZip(object obj, Stream stream)
    {
      using (GZipStream gzs = new System.IO.Compression.GZipStream(stream, CompressionMode.Compress))
        Save(obj, gzs);
    }

    public static string SaveToText<T>(T obj)
    {
      XmlWriterSettings settings = new XmlWriterSettings();
      settings.Indent = true;
      settings.OmitXmlDeclaration = true;
      settings.CheckCharacters = false;

      StringBuilder builder = new StringBuilder();

      using (XmlWriter xmlWriter = XmlWriter.Create(builder, settings))
        Save(obj, xmlWriter, typeof(T));

      return builder.ToString();
    }

    public static void SaveItems<T>(ICollection<T> items, string filename)
    {
      DataStorage<T> data = new DataStorage<T>(items);
      Save(data, filename);
    }

    public static void SaveItems<T>(string holderName, ICollection<T> items, XmlWriter writer)
    {
      if (holderName == null)
      {
        throw new ArgumentNullException("holderName");
      }
      writer.WriteStartElement(holderName);
      try
      {
        XmlSerializer xs = GetSerializer(typeof(T));
        if (items != null && items.Count > 0)
        {
          foreach (T item in items)
          {
            xs.Serialize(writer, item, emptyNamespace);
          }
        }
      }
      finally
      {
        writer.WriteEndElement();
      }
    }

    public static void SaveItemsToZip<T>(ICollection<T> items, string filename)
    {
      DataStorage<T> data = new DataStorage<T>(items);
      SaveToZip(data, filename);
    }

    public static void Save(object obj, Stream stream)
    {
      using (XmlWriter writer = CreateWriter(stream))
        Save(obj, writer);
    }

    public static void Save(object obj, TextWriter textWriter)
    {
      using (XmlWriter writer = CreateWriter(textWriter))
        Save(obj, writer);
    }

    public static void Save(object obj, XmlWriter writer)
    {
      Save(obj, writer, obj.GetType());
    }

    public static void Save(object obj, XmlWriter writer, Type type)
    {
      GetSerializer(type).Serialize(writer, obj, emptyNamespace);
    }

    static XmlWriter CreateWriter(Stream stream)
    {
      XmlWriterSettings settings = new XmlWriterSettings();
      settings.CheckCharacters = false;

      return XmlWriter.Create(stream, settings);
    }

    static XmlWriter CreateWriter(TextWriter writer)
    {
      XmlWriterSettings settings = new XmlWriterSettings();
      settings.CheckCharacters = false;

      return XmlWriter.Create(writer, settings);
    }

    static XmlSerializerNamespaces emptyNamespace = CreateEmptyNamespaces();
    static XmlSerializerNamespaces CreateEmptyNamespaces()
    {
      XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
      ns.Add("", "");
      return ns;
    }

    #endregion

    #region Loading

    public static T Load<T>(string filename)
    {
      if (!System.IO.File.Exists(filename))
        return default(T);

      using (FileStream fs = new FileStream(filename, FileMode.Open, FileAccess.Read))
      {
        return Load<T>(fs);
      }
    }

    public static T Load<T>(Stream stream)
    {
      using (XmlReader reader = CreateReader(stream))
        return (T)Load(typeof(T), reader);
    }

    public static T LoadFromZip<T>(string filename)
    {
      if (!System.IO.File.Exists(filename))
        return default(T);

      using (FileStream fs = new FileStream(filename, FileMode.Open, FileAccess.Read))
      {
        return LoadFromZip<T>(fs);
      }
    }

    public static T LoadFromZip<T>(Stream stream)
    {
      using (GZipStream gzs = new System.IO.Compression.GZipStream(stream, CompressionMode.Decompress))
      {
        return Load<T>(gzs);
      }
    }

    public static T LoadFromText<T>(string str)
    {
      using (StringReader reader = new StringReader(str))
        return (T)Load(typeof(T), reader);
    }

    public static T[] LoadItems<T>(string filename)
    {
      if (!System.IO.File.Exists(filename))
        return null;

      return Load<DataStorage<T>>(filename).Items;
    }

    public static T[] LoadItems<T>(Stream stream)
    {
      return Load<DataStorage<T>>(stream).Items;
    }

    public static T LoadFromResource<T>(Type resourceMarkerType, string name)
    {
      using (Stream stream = resourceMarkerType.Assembly.GetManifestResourceStream(resourceMarkerType, name))
      {
        if (stream == null)
          throw new Exception(string.Format("Ќе найден ресурс '{1}' (тип-маркер: '{0}')",
            resourceMarkerType.FullName, name));
        return Load<T>(stream);
      }
    }

    public static object Load(Type objectType, string filename)
    {
      if (!System.IO.File.Exists(filename))
        return null;

      using (FileStream fs = new FileStream(filename, FileMode.Open, FileAccess.Read))
        return Load(objectType, fs);
    }

    public static object Load(Type objectType, Stream stream)
    {
      using (XmlReader xmlReader = CreateReader(stream))
        return Load(objectType, xmlReader);
    }

    public static object Load(Type objectType, TextReader reader)
    {
      using (XmlReader xmlReader = CreateReader(reader))
        return Load(objectType, xmlReader);
    }


    public static object Load(Type type, XmlReader reader)
    {
      return GetSerializer(type).Deserialize(reader);
    }

    static XmlReader CreateReader(Stream stream)
    {
      XmlReaderSettings settings = new XmlReaderSettings();
      settings.CheckCharacters = false;

      return XmlReader.Create(stream, settings);
    }

    static XmlReader CreateReader(TextReader textReader)
    {
      XmlReaderSettings settings = new XmlReaderSettings();
      settings.CheckCharacters = false;

      return XmlReader.Create(textReader, settings);
    }

    #endregion

    #region Common

    public static XmlSerializer GetSerializer(Type type)
    {
      lock (criticalResource)
      {
        if (serializers.ContainsKey(type))
          return serializers[type];
        XmlSerializer serializer = new XmlSerializer(type);
        serializers[type] = serializer;
        return serializer;
      }
    }
    static object criticalResource = new object();
    static Dictionary<Type, XmlSerializer> serializers = new Dictionary<Type, XmlSerializer>();

    [System.Xml.Serialization.XmlRoot("Data")]
    public class DataStorage<TItem>
    {
      public DataStorage(ICollection<TItem> items)
      {
        this.Items = items?.ToArray();
      }
      public DataStorage() { }

      [System.Xml.Serialization.XmlElement("Item")]
      public TItem[] Items
      {
        get
        {
          if (_Items == null)
            _Items = new TItem[] { };
          return _Items;
        }
        set { _Items = value; }
      }
      TItem[] _Items;
    }

    #endregion

    public class XmlStringHolder : IXmlSerializable
    {
      public XmlStringHolder()
      {
      }

      public XmlStringHolder(string xml)
      {
        Xml = xml;
      }

      public string Xml;

      #region IXmlSerializable Members

      public System.Xml.Schema.XmlSchema GetSchema()
      {
        return null;
      }

      public void ReadXml(System.Xml.XmlReader reader)
      {
        Xml = reader.ReadInnerXml();
      }

      public void WriteXml(System.Xml.XmlWriter writer)
      {
        writer.WriteRaw(Xml);
      }

      #endregion

      public static implicit operator string(XmlStringHolder holder)
      {
        return holder != null ? holder.Xml : null;
      }

      public static implicit operator XmlStringHolder(string xml)
      {
        return new XmlStringHolder(xml);
      }
    }
  }
}

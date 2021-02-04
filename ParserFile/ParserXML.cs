using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;

using Newtonsoft.Json;

using ParserFile.Models;

namespace ParserFile
{
    public class ParserXML : IParser
    {
#region Static Fields and Constants

        private const string LocalNameSpaceCIM = "http://iec.ch/TC57/2014/CIM-schema-cim16#";
        private const string LocalNameSpaceRDF = "http://www.w3.org/1999/02/22-rdf-syntax-ns#";
        private static IParser _instance;

#endregion

#region Fields

        private XDocument _xmlDocument;

#endregion

#region Constructors

        private ParserXML(string pathXml) => PathXml = pathXml;

#endregion

#region Properties

        private string PathXml { get; }

#endregion

#region Methods

        public static IParser GetInstance(string pathXML)
        {
            _instance ??= new ParserXML(pathXML);

            return _instance;
        }

        public bool GetJsonFile() => OpenXmlDocument() && SelectData();

        private bool OpenXmlDocument()
        {
            try
            {
                _xmlDocument = XDocument.Load(PathXml);
            }
            catch (Exception)
            {
                //TODO: Logger
                return false;
            }

            return true;
        }

        private bool SelectData()
        {
            try
            {
                IList<XElement> substationXML = _xmlDocument.Root.Elements(XName.Get("Substation", LocalNameSpaceCIM)).ToList();
                IList<XElement> voltageLevelXML = _xmlDocument.Root.Elements(XName.Get("VoltageLevel", LocalNameSpaceCIM)).ToList();
                IList<XElement> synchronousMachineXML = _xmlDocument.Root.Elements(XName.Get("SynchronousMachine", LocalNameSpaceCIM)).ToList();
                IList<Substation> substationsList = new List<Substation>();

                for (int indexSubstation = 0; indexSubstation < substationXML.Count; indexSubstation++)
                {
                    Substation sub = new();
                    sub.GuidElements = substationXML[indexSubstation].Attribute(XName.Get("about", LocalNameSpaceRDF)).Value;
                    sub.NameSubstation = substationXML[indexSubstation].Element(XName.Get("IdentifiedObject.name", LocalNameSpaceCIM)).Value;
                    List<XElement> xmlElementsVoltaElementsThisSub = voltageLevelXML.Where
                                                                                        (
                                                                                         item => item.Element
                                                                                                     (XName.Get("VoltageLevel.Substation", LocalNameSpaceCIM))
                                                                                                 .Attribute(XName.Get("resource", LocalNameSpaceRDF))
                                                                                                 .Value
                                                                                                 == substationXML[indexSubstation]
                                                                                                    .Attribute(XName.Get("about", LocalNameSpaceRDF))
                                                                                                    .Value
                                                                                        )
                                                                                    .ToList();
                    IList<VoltageLevel> voltageLevelsThisSub = new List<VoltageLevel>();

                    foreach (XElement xElement in xmlElementsVoltaElementsThisSub)
                    {
                        VoltageLevel voltageLevel = new();
                        voltageLevel.NameVoltageLevel = xElement.Element(XName.Get("IdentifiedObject.name", LocalNameSpaceCIM)).Value;
                        voltageLevel.GuidElements = xElement.Attribute(XName.Get("about", LocalNameSpaceRDF)).Value;
                        List<XElement> xmlElementsSynchronousMachineThisSub = synchronousMachineXML.Where
                                                                                                       (
                                                                                                        item => item.Element
                                                                                                                    (XName.Get("Equipment.EquipmentContainer", LocalNameSpaceCIM))
                                                                                                                .Attribute(XName.Get("resource", LocalNameSpaceRDF))
                                                                                                                .Value
                                                                                                                == voltageLevel.GuidElements
                                                                                                       )
                                                                                                   .ToList();
                        IList<SynchronousMachine> synchronousMachinesThisSub = new List<SynchronousMachine>();

                        foreach (XElement element in xmlElementsSynchronousMachineThisSub)
                        {
                            SynchronousMachine synchronousMachine = new();
                            synchronousMachine.NameVoltageLevel = element.Element(XName.Get("IdentifiedObject.name", LocalNameSpaceCIM)).Value;
                            synchronousMachine.GuidElements = element.Attribute(XName.Get("about", LocalNameSpaceRDF)).Value;
                            synchronousMachinesThisSub.Add(synchronousMachine);
                        }

                        voltageLevel.SynchronousMachines = synchronousMachinesThisSub;
                        voltageLevelsThisSub.Add(voltageLevel);
                    }

                    sub.VoltageLevels = voltageLevelsThisSub;
                    substationsList.Add(sub);
                }

                string path = Path.Combine(Environment.CurrentDirectory, "RESULT.json");
                using FileStream file = new(path, FileMode.OpenOrCreate);
                using StreamWriter streamWriter = new(file, Encoding.GetEncoding(1251));
                JsonSerializerSettings jsonSerializer = new() {Culture = CultureInfo.CurrentCulture, Formatting = Formatting.Indented};
                string str = JsonConvert.SerializeObject(substationsList, jsonSerializer);
                streamWriter.Write(str);
            }
            catch (Exception)
            {
                //TODO: Logger
                return false;
            }

            return true;
        }

#endregion
    }
}
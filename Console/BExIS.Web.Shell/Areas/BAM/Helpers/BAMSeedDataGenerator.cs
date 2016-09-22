﻿using BExIS.Dlm.Entities.Party;
using BExIS.Dlm.Services.Party;
using BExIS.Security.Entities.Authentication;
using BExIS.Security.Entities.Objects;
using BExIS.Security.Services.Authentication;
using BExIS.Security.Services.Objects;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Xml;
using System.Xml.Linq;
using Vaiona.Utils.Cfg;

namespace BExIS.Web.Shell.Areas.BAM.Helpers
{
    public class BAMSeedDataGenerator
    {
        public static void GenerateSeedData()
        {
            ImportPartyTypes();
        }

        private static void ImportPartyTypes()
        {
            PartyTypeManager partyTypeManager = new PartyTypeManager();
           var filePath=Path.Combine( AppConfiguration.GetModuleWorkspacePath("DDM"),"partyTypes.xml");
            XDocument xDoc = XDocument.Load(filePath);
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(xDoc.CreateReader());
            var partyTypesNodeList = xmlDoc.SelectNodes("//PartyTypes");
            if(partyTypesNodeList.Count>0)
            foreach(XmlNode partyTypeNode in partyTypesNodeList[0].ChildNodes)
            {
                var title = partyTypeNode.Name;
                //If there is not such a party type
                if (partyTypeManager.Repo.Get(item => item.Title == title).Count == 0)
                {
                    var partyType =partyTypeManager.Create(title, "Imported from partyTypes.xml", null);
                        foreach (XmlNode customAttrNode in partyTypeNode.ChildNodes)
                        {
                            var customAttrType = customAttrNode.Attributes["type"] == null ? "String" : customAttrNode.Attributes["type"].Value;
                            partyTypeManager.CreatePartyCustomAttribute(partyType, customAttrType, customAttrNode.Name, "");
                        }
                }

            }

        }
    }
}
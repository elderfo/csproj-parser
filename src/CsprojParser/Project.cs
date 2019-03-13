// -----------------------------------------------------------------------
// <copyright file="Project.cs" company="Cloud9 Discovery LLC">
//   Copyright 2019 Cloud9 Discovery LLC. All Rights Reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace CsprojParser
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Xml.Linq;

    public class Project
    {
        private static readonly XNamespace Namespace = "http://schemas.microsoft.com/developer/msbuild/2003";
        private readonly XDocument _document;
        private readonly string _filePath;
        private IEnumerable<BuildConfiguration> _buildConfigurations;

        public Project(string filePath)
        {
            _filePath = filePath;

            using (FileStream stream = File.OpenRead(filePath))
            {
                _document = XDocument.Load(stream);
            }
        }

        public IEnumerable<BuildConfiguration> BuildConfigurations => _buildConfigurations ?? (_buildConfigurations = GetBuildConfigurations());

        public void RemoveByPlatformType(PlatformType platform)
        {
            _buildConfigurations = BuildConfigurations.Where(b => b.PlatformType != platform)
                                                      .ToList();
        }

        public void RemoveConfiguration(BuildConfiguration configuration)
        {
            _buildConfigurations = BuildConfigurations.Where(e => !e.Equals(configuration));
        }

        private static bool ContainsStringInsensitive(string haystack, string needle)
        {
            return CultureInfo.CurrentCulture
                              .CompareInfo
                              .IndexOf(haystack, needle, CompareOptions.OrdinalIgnoreCase) >= 0;
        }

        private BuildConfiguration CreateBuildConfiguration(XElement element)
        {
            string value = GetCondition(element);

            string[] values = value.Split(new[] {"|"}, StringSplitOptions.RemoveEmptyEntries);

            return new BuildConfiguration(values.First(), values.Last(), element);
        }

        private IEnumerable<BuildConfiguration> GetBuildConfigurations()
        {
            IEnumerable<XElement> configurationNodes = GetConfigurationNodes();
            foreach (XElement node in configurationNodes)
                yield return CreateBuildConfiguration(node);
        }

        private string GetCondition(XElement element)
        {
            return element.Attribute("Condition")?.Value ?? "";
        }

        private IEnumerable<XElement> GetConfigurationNodes()
        {
            IEnumerable<XElement> propertyGroups = _document.Descendants(Namespace + "PropertyGroup")
                                                            .Where(n => n.Attribute("Condition") != null)
                                                            .Where(IsBuildConfig)
                                                            .ToList();

            return propertyGroups;
        }

        private bool IsBuildConfig(XElement element)
        {
            string value = GetCondition(element);

            if (string.IsNullOrWhiteSpace(value))
                return false;

            return ContainsStringInsensitive(value, "$(Configuration)|$(PlatformType)");
        }
    }
}
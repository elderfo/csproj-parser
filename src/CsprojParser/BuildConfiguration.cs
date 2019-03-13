// -----------------------------------------------------------------------
// <copyright file="BuildConfiguration.cs" company="Cloud9 Discovery LLC">
//   Copyright 2019 Cloud9 Discovery LLC. All Rights Reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace CsprojParser
{
    using System;
    using System.Xml.Linq;

    public class BuildConfiguration
    {
        private PlatformType? _platformType;

        public BuildConfiguration(string configuration, string platform, XElement element)
        {
            Configuration = configuration;
            Platform = platform;
            Element = element;
        }

        public string Configuration { get; }

        public string Platform { get; }

        public PlatformType PlatformType
        {
            get
            {
                if (!_platformType.HasValue)
                    _platformType = GetPlatformType(Platform);

                return _platformType.Value;
            }
        }

        internal XElement Element { get; }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
                return false;
            if (ReferenceEquals(this, obj))
                return true;
            if (obj.GetType() != GetType())
                return false;
            return Equals((BuildConfiguration)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((Configuration != null ? Configuration.GetHashCode() : 0) * 397) ^ (Platform != null ? Platform.GetHashCode() : 0);
            }
        }

        private bool Equals(BuildConfiguration other)
        {
            return string.Equals(Configuration, other.Configuration) && string.Equals(Platform, other.Platform);
        }

        private PlatformType GetPlatformType(string platform)
        {
            switch (platform.ToLowerInvariant())
            {
                case "x86":
                    return PlatformType.x86;
                case "x64":
                    return PlatformType.x64;
                case "anycpu":
                    return PlatformType.AnyCpu;
                default:
                    throw new ArgumentOutOfRangeException(nameof(platform));
            }
        }
    }
}
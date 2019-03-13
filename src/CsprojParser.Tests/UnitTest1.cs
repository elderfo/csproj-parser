// -----------------------------------------------------------------------
// <copyright file="UnitTest1.cs" company="Cloud9 Discovery LLC">
//   Copyright 2019 Cloud9 Discovery LLC. All Rights Reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace CsprojParser.Tests
{
    using NUnit.Framework;

    public class Tests
    {
        [Test]
        public void Test1()
        {
            var project = new Project(
                "c:\\dev\\eda\\src\\trunk\\LexisNexis.Eda.Licensing\\LexisNexis.Eda.Licensing.Tests\\Eda.Licensing.Tests.csproj");

            

            Assert.That(project.BuildConfigurations, Has.Count.EqualTo(6));
        }
    }
}
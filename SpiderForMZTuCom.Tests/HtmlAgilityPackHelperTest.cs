// <copyright file="HtmlAgilityPackHelperTest.cs">Copyright ©  2016</copyright>
using System;
using Microsoft.Pex.Framework;
using Microsoft.Pex.Framework.Validation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SpiderForMZTuCom;

namespace SpiderForMZTuCom.Tests
{
    /// <summary>此类包含 HtmlAgilityPackHelper 的参数化单元测试</summary>
    [PexClass(typeof(HtmlAgilityPackHelper))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(InvalidOperationException))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(ArgumentException), AcceptExceptionSubtypes = true)]
    [TestClass]
    public partial class HtmlAgilityPackHelperTest
    {
    }
}

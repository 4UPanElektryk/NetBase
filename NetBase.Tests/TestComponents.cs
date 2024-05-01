using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetBase.FileProvider;
using NetBase.Templating;
using NetBase.Templating.Components;
using System.Collections.Generic;
namespace NetBase.Tests.Templating
{
	[TestClass]
	public class TestComponents
	{
		[TestMethod]
		public void Usage()
		{
			DataProvider provider = new DataProvider();
			provider.Add(new Dictionary<string, string> { { "test", "no" } });
			TComponent component = new TComponent("test")
			{
				component = "test $test$",
			};
			Assert.AreEqual(component.Use(provider), "test no");
		}
		[TestMethod]
		public void UsageComponentManager() 
		{
			#region init
			Dictionary<string,string> files = new Dictionary<string, string> 
			{
				{ "test.comp", "testt $text$" }
			};
			IFileLoader loader = new DummyFileLoader(files);
			new TComponentManager(loader);
			#endregion
			TComponent component = TComponentManager.GetComponet("test.comp");
			Assert.AreNotEqual(component.AssetName, "Dummy", "Component not found in manager!");
		}
	}
}

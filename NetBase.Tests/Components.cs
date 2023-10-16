using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetBase.Templating;
using NetBase.Templating.Components;
using System.Collections.Generic;
namespace NetBase.Tests.Templating
{
	[TestClass]
	public class Components
	{
		[TestMethod]
		public void Usage()
		{
			DataProvider provider = new DataProvider();
			provider.Data.Add(new Dictionary<string, string> { { "test", "nie" } });
			TComponent component = new TComponent("test")
			{
				component = "test $test$",
			};
			Assert.AreEqual(component.Use(provider), "test nie");
		}
		[TestMethod]
		public void UsageComponentManager() 
		{ 
		
		}
	}
}

using System;
using System.IO;
using System.Linq;
using NUnit.Framework;
using Xamarin.UITest;
using Xamarin.UITest.Queries;

namespace UITest1
{
    [TestFixture(Platform.Android)]
    [TestFixture(Platform.iOS)]
    public class Tests
    {
        IApp app;
        Platform platform;

        public Tests(Platform platform)
        {
            this.platform = platform;
        }

        [SetUp]
        public void BeforeEachTest()
        {
            app = AppInitializer.StartApp(platform);
        }

        [Test]
        public void NewTest()
        {
            string email = "ced020@latech.edu";
            string desc = "A pair of yeezys.";
            string color = "black";
            string type = "shoes";
            string loc = "The grid.";
            string fname = "Nicolas";
            string lname = "Jones";



           // app.Repl();
           // comment

            app.Tap(x => x.Text("Report An Item Lost"));
            System.Threading.Thread.Sleep(500);
            app.Tap(x => x.Text("Cancel"));
            System.Threading.Thread.Sleep(500);
            app.Tap(x => x.Text("Report An Item Lost"));
            System.Threading.Thread.Sleep(500);
            app.Tap(x => x.Text("Save"));
            app.Tap(x => x.Text("Okay"));
            app.TapCoordinates(42, 384);
            app.EnterText(email);
            app.PressEnter();
            app.TapCoordinates(42, 684);
            app.EnterText(desc);
            app.TapCoordinates(42, 984);
            app.EnterText(color);
            app.PressEnter();
            app.TapCoordinates(42, 1284);
            app.EnterText(type);
            app.PressEnter();
            app.TapCoordinates(42, 1584);
            app.EnterText(loc);
            app.PressEnter();
            app.TapCoordinates(42, 1884);
            app.Tap(x => x.Id("prev"));
            app.Tap(x => x.Id("next"));
            app.TapCoordinates(248, 1048);
            app.TapCoordinates(548, 1048);
            app.TapCoordinates(748, 1048);
            app.TapCoordinates(448, 1148);
            app.Tap(x => x.Text("OK"));
            app.ScrollDownTo(x => x.Text("Last Name"));
            app.TapCoordinates(42, 1684);
            app.EnterText(fname);
            app.PressEnter();
            app.TapCoordinates(42, 1984);
            app.EnterText(lname);
            app.PressEnter();
            app.Tap(x => x.Text("Save"));
            app.Tap(x => x.Text("Okay"));
            System.Threading.Thread.Sleep(1000);

            app.TapCoordinates(28, 100);
            app.Tap(x => x.Text("About"));
            System.Threading.Thread.Sleep(500);
            app.TapCoordinates(28, 100);
            app.Tap(x => x.Text("Admin"));
            System.Threading.Thread.Sleep(500);
            app.TapCoordinates(264, 1092);
            app.EnterText("8");
            app.EnterText("6");
            app.EnterText("7");
            app.EnterText("1");
            app.PressEnter();
            System.Threading.Thread.Sleep(700);
            app.Tap(x => x.Text("View Lost Items"));
            System.Threading.Thread.Sleep(1000);
            app.TapCoordinates(28, 100);
            System.Threading.Thread.Sleep(500);
            app.TapCoordinates(28, 100);
            app.Tap(x => x.Text("Home"));

        }
    }
}

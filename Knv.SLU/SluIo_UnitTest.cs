﻿
namespace Knv.SLU
{

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using BitwiseSystems;
    using NUnit.Framework;
    using System.Diagnostics;
    using System.Reflection;


    [TestFixture]
    internal class SluIo_UnitTes
    {
        string LOG_ROOT_DIR = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

        [Test]
        public void Slu0AttachCheck()
        {
            var devname = SluIo.GetAttachedNameOfUnits();
            Assert.IsTrue(devname.Contains("QUSB-0"));
        }

        [Test]
        public void SluCount()
        {
            var devname = SluIo.GetAttachedNameOfUnits();
            Assert.IsTrue(devname.Contains("QUSB-0"));
        }


        [Test]
        public void SluVenturiCardsPresent()
        {
            var cards = new List<string>();
            using (var slu = new SluIo())
            {
                slu.Open();
                {
                    int row = 0;
                    for (int unit = 0; unit < 2; unit++)
                    {
                        for (int slot = 1; slot <= 21; slot++)
                        {
                            row++;
                            var type = slu.ReadRegister((byte)unit, (byte)slot, 0);
                            string name = "";
                            slu.CardTypes.TryGetValue(type, out name);
                            cards.Add($"{row}. SLU{unit}, Slot: {slot}, Card Type:{name} - {type:X2} ");
                        }
                    }
                }

                slu.LogSave(LOG_ROOT_DIR, MethodBase.GetCurrentMethod().Name);
            }
            foreach (var card in cards)
                TestContext.Out.WriteLine(card);
        }

        [Test]
        public void IsSlu0InstCardIsPresent()
        {
            using (var slu = new SluIo())
            {
                slu.Open();
                {
                    var cardtype = slu.ReadRegister(0, 21, 0);
                    Assert.AreEqual(0x43, cardtype);
                }
            }
        }

        [Test]
        public void SortOfSluNames()
        {
            string[] names = new string[] { "QUSB-2", "QUSB-1", "QUSB-0", "QUSB-3" };
            Array.Sort(names, StringComparer.CurrentCultureIgnoreCase);
            Assert.AreEqual(new string[] { "QUSB-0", "QUSB-1", "QUSB-2", "QUSB-3" }, names);
        }



    }
}

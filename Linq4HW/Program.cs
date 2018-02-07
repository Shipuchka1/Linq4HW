using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Linq4HW.Model;
using Linq4HW;
using System.Xml.Linq;
using System.IO;

namespace Linq4HW
{
    
    class Program
    {
        public static Model.Model1 db = new Model.Model1();
          static void Main(string[] args)
        {
            Task12();
        }

        public static void Task1()
        {
            // a.Все зоны/ участки которые принадлежат PavilionId = 1, при этом каждая зоны должна находиться в отдельном XML файле с наименованием PavilionId.

            var query = db.Areas.Where(w => w.PavilionId == 1);

            foreach (var item in query)
            {
                XDocument xDoc = new XDocument
                    (
                    new XElement("Area",
                  new XElement("AreaId", item.AreaId),
                  new XElement ("Name", item.Name),
                  new XElement ("IP", item.IP)
                      )
                  );

                xDoc.Save(@"Task1\"+item.AreaId + ".xml");
            }
        }

        public static void Task2()
        {
            //b.Используя Directory класс, создать папки с название зон / участков, в каждую папку выгрузить XML файл на основе данных их таблицы.

            foreach(var item in db.Areas.ToList())
            {
                DirectoryInfo dir = new DirectoryInfo(@"Task2\"+item.Name);
                dir.Create();
                XDocument xDoc = new XDocument
                    (
                    new XElement("Area",
                  new XElement("AreaId", item.AreaId),
                  new XElement("Name", item.Name),
                  new XElement("IP", item.IP)
                      )
                  );
                xDoc.Save(@dir.FullName+@"\" + item.AreaId + ".xml");
            }
        }
        public static void Task3()
        {
            // c.Выгрузить XML файл только тех участков, которые не имеют дочерних элементов (ParentId = 0);

            var query = db.Areas.Where(w => w.ParentId == 0);

            XDocument xDoc = new XDocument();
            XElement root = new XElement("Areas");
            foreach (var item in query)
            {

                root.Add(
                    new XElement("Area",
                    new XElement("FullName", item.FullName),
                    new XElement("IP", item.IP),
                    new XElement("AreaId", item.AreaId),
                    new XElement("Name", item.Name),
                    new XElement("ParentId", item.ParentId)
                 
                     )
                 );


            }
            xDoc.Add(root);
            xDoc.Save(@"Task3\Doc.xml");
        }

        public static void Task4()
        {
            //d.Выгрузить из таблицы Timer, данные только для 
            //    зон у которых есть IP адрес, при этом в XML
            //    файл должны войти следующие поля: UserId, 
            //Area Name (name из Талицы Area), DateStart

            var query = db.Areas.Where(w => !string.IsNullOrEmpty(w.IP));
            var res = query.Join(db.Timers, q => q.AreaId, t => t.AreaId, (area, time) => new { area.Name, time.UserId, time.DateStart, time.TimerId });
            foreach (var item in res)
            {
                XDocument xDoc = new XDocument
                    (
                    new XElement("Area",
                  new XElement("UserId", item.UserId),
                  new XElement("Name", item.Name),
                  new XElement("DateStart", item.DateStart)
                      )
                  );

                xDoc.Save(@"Task4\" + item.TimerId + ".xml");
            }
        }

        public static void Task5()
        {
            // e.Выгрузить в XML файл, данные из таблицы Timer, у которых нет даты завершения работы DateFinish = null

            var query = db.Timers.Where(w => string.IsNullOrEmpty(w.DateFinish.ToString()));

           

            XDocument xDoc = new XDocument();
            XElement root = new XElement("Timers");
            foreach (var item in query)
            {

                root.Add(
                    new XElement("Timer",
                    new XElement("UserId", item.UserId),
                    new XElement("DocumentId", item.DocumentId),
                    new XElement("AreaId", item.AreaId),
                 new XElement("DateStart", item.DateStart),
                 new XElement("DateFinish", item.DateFinish)
                     )
                 );


            }
            xDoc.Add(root);
            xDoc.Save(@"Task5\Doc.xml");

        }

        public static void Task6()
        {
            // f.Выгрузить весь список выполненных работ из таблицы Timer
            var query = db.Timers.Where(w => !string.IsNullOrEmpty(w.DateFinish.ToString()));

            XDocument xDoc = new XDocument();
            XElement root = new XElement("Timers");
            foreach (var item in query)
            {

                root.Add(
                    new XElement("Timer",
                    new XElement("UserId", item.UserId),
                    new XElement("DocumentId", item.DocumentId),
                    new XElement("AreaId", item.AreaId),
                 new XElement("DateStart", item.DateStart),
                 new XElement("DateFinish", item.DateFinish)
                     )
                 );


            }
            xDoc.Add(root);
            xDoc.Save(@"Task6\Doc.xml");
        }
        
        public static void Task7()
        {
            //g.Выгрузить данные с таблицы Area в переменную,
            //на основе данных в этой переменной создать XML файл
            //имеющим Xmlns = «area», а также namespace - http://logbook.itstep.org/

            var query = db.Areas.ToList();
            XDocument xDoc = new XDocument();
            XElement root = new XElement("Areas", new XAttribute("Xmlns", "area"), new XAttribute("namespace", "http://logbook.itstep.org/"));
            foreach (var item in query)
            {
               
                 root.Add( 
                     new XElement("Area",
                     new XElement("AreaId", item.AreaId),
                  new XElement("Name", item.Name),
                  new XElement("IP", item.IP)
                      )
                  );
                
                
            }
            xDoc.Add(root);
            xDoc.Save(@"Task7\Doc.xml");

        }

        public static void Task8()
        {
            XDocument XDoc = XDocument.Load(@"Task6\Doc.xml");
            foreach (XElement item in XDoc.Root.Nodes())
            {
                Console.WriteLine(item.ToString());
                
            }
        }

        public static void Task9()
        {
            //b.Выгрузить все данные из XML пункта F, 
            //заменить при этом в XML файла DateFinish = текущая дата
            //и сохранить данный XML файл с наименованием – «TimeChangeToday_10.27.2017»
            XDocument XDoc = XDocument.Load(@"Task6\Doc.xml");
            foreach (XElement item in XDoc.Root.Nodes())
            {
                item.SetElementValue("DateFinish", DateTime.Now);
            }

            XDoc.Save(@"Task9\TimeChangeToday_10.27.2017.xml");
        }

        public static void Task10()
        {
            //c.Вывести на экран, данные из XML пункта С, из веток – AreaId, Name, FullName, IP
            XDocument XDoc = XDocument.Load(@"Task3\Doc.xml");

            foreach (XElement item in XDoc.Root.Nodes())
            {
                Console.WriteLine("AreaId - {0}, Name - {1}, FullName - {2}, IP - {3}",item.Element("AreaId").Value,item.Element("Name").Value,item.Element("FullName").Value,item.Element("IP").Value);
                Console.WriteLine();

            }
        }

        public static void Task11()
        {
            //d.Выгрузить из XML файла (XML из пункта G) на экран,
            // только те, у которых UserId != 0,
            //а так же нет даты завершения DateFinish = null.

            XDocument XDoc = XDocument.Load(@"Task5\Doc.xml");

            foreach (XElement item in XDoc.Root.Nodes())
            {
                if (item.Element("UserId").Value != "0")
                    Console.WriteLine(item);

            }   
        }

        public static void Task12()
        {


            //5.Выгрузить статистику на экран используя данные из XML файла пункта C
            //a.Количеств не завершенных работ
            //b.Количество работ на каждой зоне с указанием наименования зоны.
            //c.Сумму потраченного времени на каждой зоне
            //d.Данные описанные пунктом выше с «a» по «c» -должны быть отсортированы от большего к меньшему.

            //Загрузим Areas из @"Task7\Doc.xml"
            XDocument XDocAreas = XDocument.Load(@"Task7\Doc.xml");
            //Зарузим Timers из @"Task5\Doc.xml" и @"Task6\Doc.xml"
            XDocument XDocTimersUnfinished = XDocument.Load(@"Task5\Doc.xml");
            XDocument XDocTimersFinished = XDocument.Load(@"Task6\Doc.xml");
            XElement Timers = new XElement("Timers");
            Timers.Add(XDocTimersUnfinished.Root.Nodes());
            Timers.Add(XDocTimersFinished.Root.Nodes());

            //a.Количеств не завершенных работ
            int tc;
            tc = Timers.Elements("Timer").Where(w => string.IsNullOrEmpty(w.Element("DateFinish").Value)).Count();
            Console.WriteLine("\na.Количеств не завершенных работ: {0}\n", tc);

            //b.Количество работ на каждой зоне с указанием наименования зоны.
            var grouped = XDocAreas.Root.Elements("Area").Join(
                Timers.Elements("Timer"),
                a => a.Element("AreaId").Value,
                t => t.Element("AreaId").Value,
                (area, timer) => new XElement("Area", area.Element("Name"), area.Element("AreaId"))
                ).GroupBy(
                a => a.Element("AreaId").Value,
                (area, areas) => new XElement("GArea", areas.ElementAt(0).Nodes(), new XElement("Count", areas.Count()))
                ).OrderByDescending(
                 a => Int32.Parse(a.Element("AreaId").Value)
                );

            XElement JobCounts = new XElement("Areas", grouped);
            Console.WriteLine("\nb.Количество работ на каждой зоне с указанием наименования зоны:\n {0}", JobCounts.ToString());



            //c.Сумму потраченного времени на каждой зоне
            var tgrouped = XDocAreas.Root.Elements("Area").Join(
                            Timers.Elements("Timer"),
                            a => a.Element("AreaId").Value,
                            t => t.Element("AreaId").Value,
                            (area, timer) => new XElement("Area", area.Element("Name"),
                                  area.Element("AreaId"),
                                  new XElement("Duration", (string.IsNullOrEmpty(timer.Element("DateFinish").Value) ? "0" : (DateTime.Parse(timer.Element("DateFinish").Value) - DateTime.Parse(timer.Element("DateStart").Value)).TotalSeconds.ToString())))
                            ).GroupBy(
                            a => a.Element("AreaId").Value,
                            (area, areas) => new XElement("TArea", areas.ElementAt(0).Element("Name"), areas.ElementAt(0).Element("AreaId"), new XElement("Sum", areas.Sum(a=>Double.Parse(a.Element("Duration").Value))))
                            ).OrderByDescending(
                             a => Int32.Parse(a.Element("AreaId").Value)
                            );

            XElement JobTime = new XElement("Areas", tgrouped);
            Console.WriteLine("\nc.Сумму потраченного времени на каждой зоне:\n {0}", JobTime.ToString());

        }
    }
}

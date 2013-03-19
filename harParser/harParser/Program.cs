using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Google.GData.Client;
using Google.GData.Extensions;
using Google.GData.Spreadsheets;
using Newtonsoft.Json;

namespace harParser
{
    class Program
    {
        private static string buildEntry(string entry)
        {
            if (string.IsNullOrWhiteSpace(entry))
            {
                entry = "-1";
            }
            return @"""" + entry +@""",";
        }

        static void Main(string[] args)
        {
            Console.WriteLine("Input google username:");
            string userName = Console.ReadLine();
            Console.WriteLine("Input google password:");
            string password = Console.ReadLine();
            Console.WriteLine("Input spreadsheet to enumerate");
            string spreadsheetName = string.Empty;// Console.ReadLine();
            Console.WriteLine("Sheet to use");
            string sheetName = string.Empty;// Console.ReadLine();

            string csvString = string.Empty;

            if (string.IsNullOrEmpty(spreadsheetName))
            {
                spreadsheetName = "";
            }

            if (string.IsNullOrWhiteSpace(sheetName))
            {
                sheetName = "Sheet1";
            }
            
            SpreadsheetsService service = new SpreadsheetsService("rightscale.com");
            service.setUserCredentials(userName, password);

            SpreadsheetQuery query = new SpreadsheetQuery();
            SpreadsheetFeed feed = service.Query(query);

            Console.WriteLine("Spreadsheets:");

            AtomLink spreadSheetLink = null;

            foreach (SpreadsheetEntry se in feed.Entries)
            {
                Console.WriteLine("  " + se.Title.Text);
                if (se.Title.Text == spreadsheetName)
                {
                    spreadSheetLink = se.Links.FindService(GDataSpreadsheetsNameTable.WorksheetRel, null);
                }
            }

            if (spreadSheetLink != null)
            {
                WorksheetQuery wsQuery = new WorksheetQuery(spreadSheetLink.HRef.ToString());
                WorksheetFeed wsFeed = service.Query(wsQuery);

                AtomLink cellFeedLink = null;

                foreach (WorksheetEntry we in wsFeed.Entries)
                {
                    Console.WriteLine(" --" + we.Title.Text);
                    if (we.Title.Text == sheetName)
                    {
                        cellFeedLink = we.Links.FindService(GDataSpreadsheetsNameTable.CellRel, null);
                    }
                }

                if (cellFeedLink != null)
                {
                    CellQuery cellQuery = new CellQuery(cellFeedLink.HRef.ToString());
                    CellFeed cellFeed = service.Query(cellQuery);

                    uint currRow = 2;

                    Guid rowID = Guid.NewGuid();
                    csvString += buildEntry(rowID.ToString() );

                    foreach (CellEntry curCell in cellFeed.Entries)
                    {
                        if (curCell.Row > 1)
                        {
                            if (curCell.Row != currRow)
                            {
                                rowID = Guid.NewGuid();
                                csvString += Environment.NewLine + buildEntry(rowID.ToString() );
                                currRow = curCell.Row;
                            }
                            if (curCell.Cell.Column != 6)
                            {
                                csvString += buildEntry(curCell.Value );
                            }
                            else
                            {
                                using(System.IO.StreamWriter sw = new System.IO.StreamWriter(@"D:\dev\harfiles\" + rowID.ToString() + ".har"))
                                {
                                    sw.Write(curCell.Cell.Value);
                                    sw.Flush();
                                }
                            }
                        }
                    }
                    if (!string.IsNullOrWhiteSpace(csvString))
                    {
                        using (System.IO.StreamWriter sw = new System.IO.StreamWriter(@"D:\dev\harfiles\_harHeaders.csv"))
                        {
                            sw.Write(csvString);
                            sw.Flush();
                        }
                    }
                }
            }
            string[] files = System.IO.Directory.GetFiles(@"D:\dev\harfiles", "*.har");
            if(files != null && files.Length > 0)
            {
                foreach(string f in files)
                {
                    string testID = f.Replace(".har", "").Replace(@"D:\dev\harfiles\", "");
                    HarFile hf = null;
                    using (System.IO.StreamReader sr = new System.IO.StreamReader(f))
                    {
                        string contents = sr.ReadToEnd();
                        hf = JsonConvert.DeserializeObject<HarFile>(contents);
                    }
                    string dataFileContents = string.Empty;
                    foreach (var entries in hf.log.entries)
                    {
                        string dataline = string.Empty;
                        dataline += buildEntry(entries.request.url );
                        dataline += buildEntry(entries.request.method );
                        dataline += buildEntry(entries.request.headersSize.ToString() );
                        dataline += buildEntry(entries.request.bodySize.ToString() );
                        dataline += buildEntry(entries.time.ToString() );
                        dataline += buildEntry(entries.timings.dns.ToString() );
                        dataline += buildEntry(entries.timings.connect.ToString() );
                        dataline += buildEntry(entries.timings.send.ToString());
                        dataline += buildEntry(entries.timings.blocked.ToString());
                        dataline += buildEntry(entries.timings.wait.ToString() );
                        dataline += buildEntry(entries.timings.ssl.ToString() );
                        dataline += buildEntry(entries.timings.receive.ToString() );
                    
                        dataline += buildEntry(entries.response.bodySize.ToString());
                        dataline += buildEntry(entries.response.status.ToString() );
                        dataline += buildEntry(entries.response.statusText );
                        dataFileContents += dataline + Environment.NewLine;
                    }

                    using (System.IO.StreamWriter sw = new System.IO.StreamWriter(@"d:\dev\harfiles\_hardetail.csv"))
                    {
                        sw.Write(dataFileContents);
                        sw.Flush();
                    }
                }
            }
        }
    }
}


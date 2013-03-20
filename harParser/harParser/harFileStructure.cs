using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace harParser
{
    public class Creator
    {
        public string name { get; set; }
        public string version { get; set; }
    }

    public class PageTimings
    {
        public string onContentLoad { get; set; }
        public string onLoad { get; set; }
    }

    public class Page
    {
        public string startedDateTime { get; set; }
        public string id { get; set; }
        public string title { get; set; }
        public PageTimings pageTimings { get; set; }
    }

    public class Param
    {
        public string name { get; set; }
        public string value { get; set; }
    }

    public class PostData
    {
        public string mimeType { get; set; }
        public string text { get; set; }
        public List<Param> @params { get; set; }
    }

    public class Request
    {
        public string method { get; set; }
        public string url { get; set; }
        public string httpVersion { get; set; }
        public List<object> headers { get; set; }
        public List<object> queryString { get; set; }
        public List<object> cookies { get; set; }
        public int headersSize { get; set; }
        public int bodySize { get; set; }
        public PostData postData { get; set; }
    }

    public class Content
    {
        public int size { get; set; }
        public string mimeType { get; set; }
        public int compression { get; set; }
    }

    public class Response
    {
        public int status { get; set; }
        public string statusText { get; set; }
        public string httpVersion { get; set; }
        public List<object> headers { get; set; }
        public List<object> cookies { get; set; }
        public Content content { get; set; }
        public string redirectURL { get; set; }
        public int headersSize { get; set; }
        public int bodySize { get; set; }
    }

    public class Cache
    {
    }

    public class Timings
    {
        public string blocked { get; set; }
        public string dns { get; set; }
        public string connect { get; set; }
        public string send { get; set; }
        public string wait { get; set; }
        public string receive { get; set; }
        public string ssl { get; set; }
    }

    public class Entry
    {
        public string startedDateTime { get; set; }
        public int time { get; set; }
        public Request request { get; set; }
        public Response response { get; set; }
        public Cache cache { get; set; }
        public Timings timings { get; set; }
        public string pageref { get; set; }
    }

    public class Log
    {
        public string version { get; set; }
        public Creator creator { get; set; }
        public List<Page> pages { get; set; }
        public List<Entry> entries { get; set; }
    }

    public class HarFile
    {
        public Log log { get; set; }
    }
}

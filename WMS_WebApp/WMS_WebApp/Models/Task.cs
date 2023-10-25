namespace WMS_WebApp.Models
{
    public class Task
    {
        public long id { get; set; }

        public string type { get; set; }

        public string item_id { get; set; }

        public string loc_id { get; set; }

        public string machine_id { get; set; }

        public string lot_id { get; set; }

        public string status { get; set; }

        // Either "STORE STARTED" , "STORE COMPLETED" and "STORE FAULT"
        public string message { get; set; }


    }
}

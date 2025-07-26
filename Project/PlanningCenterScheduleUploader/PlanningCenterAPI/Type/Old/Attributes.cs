using PlanningCenterAPI.Type.Part;

namespace PlanningCenterAPI.Type.Old
{
    public class Attributes
    {
        public string archived_at { get; set; }
        public bool attachment_types_enabled { get; set; }
        public string background_check_permissions { get; set; }
        public string comment_permissions { get; set; }
        //public List<> custom_item_types { get; set; }
        public string deleted_at { get; set; }
        public string frequency { get; set; }
        public string last_plan_from { get; set; }
        public string name { get; set; }
        public string permissions { get; set; }
        public bool scheduled_publish { get; set; }
        public int sequence { get; set; }
        public List<Standard_Item_Types> standard_item_types { get; set; }
        public string updated_at { get; set; }
    }
}

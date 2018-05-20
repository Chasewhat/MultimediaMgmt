namespace MultimediaMgmt.Common.Config
{
    /// <summary>
    /// 系统配置项,为固定选项,对于每一个配置项,预定义属性并提供读写接口
    /// </summary>
    public class MainConfig : ConfigBase
    {
        /// <summary>
        /// Web通信地址
        /// </summary>
        public string WebUrl
        {
            get
            {
                return Get("WEBURL");
            }
            set
            {
                Set("WEBURL", value);
            }
        }

        public string Buildings1
        {
            get
            {
                return Get("BUILDINGS1");
            }
            set
            {
                Set("BUILDINGS1", value);
            }
        }

        public string Buildings2
        {
            get
            {
                return Get("BUILDINGS2");
            }
            set
            {
                Set("BUILDINGS2", value);
            }
        }

        public string ClassRooms
        {
            get
            {
                return Get("CLASSROOMS");
            }
            set
            {
                Set("CLASSROOMS", value);
            }
        }
    }
}
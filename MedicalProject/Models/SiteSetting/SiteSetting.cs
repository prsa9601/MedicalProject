namespace MedicalProject.Models.SiteSetting
{
  
        public class SiteSetting : BaseDto
        {
            public string CompanyName { get; set; }


            private static SiteSetting _instance;
            private SiteSetting() { }


            public static SiteSetting Instance
            {
                get
                {
                    if (_instance == null)
                    {
                        _instance = new SiteSetting();
                    }
                    return _instance;
                }
            }

            public void SetCompanyName(string companyName)
            {
                CompanyName = companyName;
            }
        }
    public class CreateOrEditCommand
    {
        public required string ComparyName { get; set; }
    }
}

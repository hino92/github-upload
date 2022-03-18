using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GpresultAdjust
{
    class AdministrativeTemplateData : IComparable
    {
        public string m_GPO = null;
        public string m_Folder_id = null;
        public string m_Value = null;
        public string m_State= null;

        public AdministrativeTemplateData ()
        {

        }

        public string ToText()
        {
            string value = string.Empty;
            value = value + m_GPO + "\n";
            value = value + m_Folder_id + "\n";
            value = value + m_Value + "\n";
            value = value + m_State + "\n";
            return value;
        }

        
        public int CompareTo(object obj)
        {
            AdministrativeTemplateData adt = (AdministrativeTemplateData)obj;

            //大きさは m_Folder_id の内容だけで決める。
            return m_Folder_id.CompareTo(adt.m_Folder_id);

        }
    }
}

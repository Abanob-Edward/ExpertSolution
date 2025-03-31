using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAO.Interfaces
{
    public interface IFormsDAO
    {
        public int SaveProfessionalQualifications(ProfessionalQualifications entity);
        public int SaveExpertQuestionAnswers(List<ExpertQuestionAnswer> answers);
        public List<ExpertQuestionAnswer> getExpertQuestionAnswers(string ExpertGUid);
        public List<ProfessionalQualifications> GetALLProfessionalQualifications(string UserGUID);
        public ProfessionalQualifications GetByIdProfessionalQualifications(int id);
        public int DeleteProfessionalQualifications(int id);

        public int SaveAcademicQualifications(AcademicQualifications entity);
        public List<AcademicQualifications> GetALLAcademicQualifications(string UserGUID);

        public AcademicQualifications GetByIdAcademicQualifications(int id);

        public int DeleteAcademicQualifications(int id);

        public int SaveCertificateQualifications(CertificateQualifications entity);
        public List<CertificateQualifications> GetALLCertificateQualifications(string UserGUID);

        public CertificateQualifications GetByIdCertificateQualifications(int id);

        public int DeleteCertificateQualifications(int id);
        public int CreateTableRow(TableForms entity);
        public int DeleteTableRow(TableForms entity);
        public int UpdateTableField(TableForms entity);


    }
}

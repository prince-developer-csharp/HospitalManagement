using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalManagement
{
    class Program
    {
        static void Main(string[] args)
        {
            string name;
            int age;
            string gender;
            long number;
            string address;
            string disease;
            bool loop = true;
            int patientId = 0;
            string departmentName;
            char addDoctor = 'y';
            int departmentId = 0;
            string doctorName;
            string treatment;
            char addAsistant = 'y';
            string assistantName;
            string professionName;
            string mobileNumber;
            string drug;
            char addDepartment = 'y';
            var dbContext = new HospitalDbEntities();
            while (loop)
            {
                Console.WriteLine("1.Add patient");
                Console.WriteLine("2.Show Patient Details which type of treatment take");
                Console.WriteLine("3.Add department, doctor and health assistant");
                Console.WriteLine("4.Add Drug");
                Console.WriteLine("5.Delete Drug by name");
                Console.WriteLine("6.Show Department");
                Console.WriteLine("7.Show Doctor");
                Console.WriteLine("8.Exit");
                Console.WriteLine("Enter your choice : ");
                int choice = Convert.ToInt32(Console.ReadLine());
                switch (choice)
                {
                    case 1:
                        Console.WriteLine("Enter the patient name: ");
                        name = Console.ReadLine();
                        Console.WriteLine("Enter the age:");
                        age = Convert.ToInt32(Console.ReadLine());
                        Console.WriteLine("Enter the gender : ");
                        gender = Console.ReadLine();
                        Console.WriteLine("Enter the Contact Number");
                        number = Convert.ToInt64(Console.ReadLine());
                        Console.WriteLine("Enter the address : ");
                        address = Console.ReadLine();
                        Console.WriteLine("Enter the disease : ");
                        disease = Console.ReadLine();
     
                        var patient = new Patient()
                        {
                            PatientName = name,
                            Age = age,
                            Gender = gender,
                            ContactNumber = number,
                            Address = address,
                            DiseaseName = disease
                        };

                        var checkPatient = dbContext.Patients.Where(t => t.PatientName == name && t.ContactNumber == number).FirstOrDefault();
                        if(checkPatient == null)
                        {

                        }
                        else
                        {
                            Console.WriteLine("patient already inserted");
                            return;
                        }
                        try
                        {
                            dbContext.Patients.Add(patient);
                            dbContext.SaveChanges();
                        }
                        catch(Exception e)
                        {
                            Console.WriteLine(e);
                        }

                        //find patient Id 
                        var findPatient = dbContext.Patients.Where(t => t.PatientName == name).FirstOrDefault();
                        if(findPatient == null)
                        {
                            Console.WriteLine("");
                        }
                        else
                        {
                            patientId = findPatient.PatientId;
                        }

                        //find all doctor for disease
                        var findDoctor = dbContext.Doctors.ToList().Where(t => t.treatment == disease);
                        Console.WriteLine(findDoctor);
                        var count = findDoctor.Count();
                        Console.WriteLine(count);
                        if (count == 0)
                        {
                            Console.WriteLine("Treatment is not available for disease");
                            //delete patient if treatment is not available
                            try
                            {
                                dbContext.Patients.Remove(dbContext.Patients.Single(t => t.PatientId == patientId));
                                dbContext.SaveChanges();
                            }
                            catch(Exception e)
                            {
                                Console.WriteLine(e);
                            }
                        }
                        else
                        {
                            foreach(var doctor in findDoctor)
                            {
                                Console.WriteLine(doctor.DoctorId + ". " + doctor.DoctorName);
                            }

                            Console.WriteLine("Enter your doctorID : ");
                            int doctorId = Convert.ToInt32(Console.ReadLine());

                            var assignDoctor = new Appointment()
                            {
                                PatientId = patientId,
                                DoctorId = doctorId
                            };

                            try
                            {
                                dbContext.Appointments.Add(assignDoctor);
                                dbContext.SaveChanges();
                            }

                            catch(Exception e)
                            {
                                Console.WriteLine(e);
                            }
                        }
                        break;

                    case 2:
                        Console.WriteLine("Enter the patient name: ");
                        name = Console.ReadLine();
                        Console.WriteLine("Enter the Contact Number");
                        number = Convert.ToInt64(Console.ReadLine());
                        var patientDetails = dbContext.vGetPatientDetails.Where(t => t.PatientName == name && t.ContactNumber == number).FirstOrDefault();
                        Console.WriteLine("Patient Name : " + patientDetails.PatientName);
                        Console.WriteLine("Patient Gender : " + patientDetails.Gender);
                        Console.WriteLine("Patient Age : " + patientDetails.Age);
                        Console.WriteLine("Patient Number : " + patientDetails.ContactNumber);
                        Console.WriteLine("Patient Address : " + patientDetails.Address);
                        Console.WriteLine("Doctor Name : " + patientDetails.DoctorName);
                        Console.WriteLine("Treatment: " + patientDetails.treatment);
                        Console.WriteLine("Department Name : " + patientDetails.DepartmentName);
                        Console.WriteLine("Assistant Name : " + patientDetails.AssistantName);
                        break;

                    case 3:
                        Console.WriteLine("Do You wanna add department y/n : ");
                        addDepartment = Convert.ToChar(Console.ReadLine());
                        while(addDepartment == 'y')
                        {

                            Console.WriteLine("Enter the department name: ");
                            departmentName = Console.ReadLine();
                            var department = new Department()
                            {
                                DepartmentName = departmentName
                            };

                            //find department exist or not
                            var checkDepartment = dbContext.Departments.Where(t => t.DepartmentName == departmentName).FirstOrDefault();

                            if (checkDepartment == null)
                            {
                                try
                                {
                                    dbContext.Departments.Add(department);
                                    dbContext.SaveChanges();
                                }
                                catch (Exception e)
                                {
                                    Console.WriteLine(e);
                                }
                            }
                            else
                            {
                                Console.WriteLine("department is already here");
                            }
                            Console.WriteLine("Do You wanna add department : ");
                            addDepartment = Convert.ToChar(Console.ReadLine());
                        }
                       
                        Console.WriteLine("Do you want to add doctor y/n");
                        addDoctor = Convert.ToChar(Console.ReadLine());
                        while (addDoctor == 'y')
                        {

                            //get list of departments
                            var departmentlist = dbContext.Departments.ToList();
                            Console.WriteLine("Department List");
                            foreach (var departments in departmentlist)
                            {
                                Console.WriteLine(departments.DepartmentId + ". " + departments.DepartmentName);
                            }

                            Console.WriteLine("Enter the department Id to add doctor : ");
                            departmentId = Convert.ToInt32(Console.ReadLine());
                            Console.WriteLine("Enter the doctor name : ");
                            doctorName = Console.ReadLine();
                            Console.WriteLine("Enter the treatment name doctor do : ");
                            treatment = Console.ReadLine();
                            Console.WriteLine("Enter the doctor mobile Number : ");
                            mobileNumber = Console.ReadLine();

                            var doctor = new Doctor()
                            {
                                DoctorName = doctorName,
                                DepartmentId = departmentId,
                                treatment = treatment,
                                ContactNumber = mobileNumber
                            };

                            //check doctor is exist or not with the help of mobile number
                            var checkDoctor = dbContext.Doctors.Where(t => t.ContactNumber == mobileNumber).FirstOrDefault();
                            if (checkDoctor == null)
                            {
                                try
                                {
                                    dbContext.Doctors.Add(doctor);
                                    dbContext.SaveChanges();
                                }
                                catch (Exception e)
                                {
                                    Console.WriteLine(e);
                                }

                                Console.WriteLine("Doctor inserted successfully");
                            }
                            else
                            {
                                Console.WriteLine("Doctor already inserted");
                            }
                            

                            Console.WriteLine("Do you want to add doctor y/n");
                            addDoctor = Convert.ToChar(Console.ReadLine());
                        }

                        Console.WriteLine("Do you want to add assistant y/n");
                        addAsistant = Convert.ToChar(Console.ReadLine());
                        while (addAsistant == 'y')
                        {
                            //get list of departments
                            var departmentLIST = dbContext.Departments.ToList();
                            Console.WriteLine("Department List");
                            foreach (var departments in departmentLIST)
                            {
                                Console.WriteLine(departments.DepartmentId + ". " + departments.DepartmentName);
                            }

                            Console.WriteLine("Enter the department Id to add assistant : ");
                            departmentId = Convert.ToInt32(Console.ReadLine());
                            Console.WriteLine("Enter the assistant name : ");
                            assistantName = Console.ReadLine();
                            Console.WriteLine("Enter the profession Name : ");
                            professionName = Console.ReadLine();
                            Console.WriteLine("Enter the doctor mobile Number : ");
                            mobileNumber = Console.ReadLine();
                            var assistant = new HealthcareAssistant()
                            {
                                AssistantName = assistantName,
                                ProfessionName = professionName,
                                DepartmentId = departmentId,
                                ContactNumber = mobileNumber
                            };

                            //check assistant is already exist or not with the help of mobile number

                            var checkAssistant = dbContext.HealthcareAssistants.Where(t => t.ContactNumber == mobileNumber).FirstOrDefault();
                            if(checkAssistant == null)
                            {
                                try
                                {
                                    dbContext.HealthcareAssistants.Add(assistant);
                                    dbContext.SaveChanges();
                                }

                                catch (Exception e)
                                {
                                    Console.WriteLine(e);
                                }

                                Console.WriteLine("Assistant inserted successfully");
                            }
                            else
                            {
                                Console.WriteLine("Assitant already inserted");
                            }

                            Console.WriteLine("Do you want to add assistant y/n");
                            addAsistant = Convert.ToChar(Console.ReadLine());
                        }

                        break;

                    case 4:
                        Console.WriteLine("Enter the drug name: ");
                        drug = Console.ReadLine();
                        var drugs = new Drug()
                        {
                            DrugName = drug
                        };

                        //check drug is exist or not with the help of drug name
                        var checkDrug = dbContext.Drugs.Where(t => t.DrugName == drug).FirstOrDefault();
                        if(checkDrug == null)
                        {
                            try
                            {
                                dbContext.Drugs.Add(drugs);
                                dbContext.SaveChanges();
                            }catch(Exception e)
                            {
                                Console.WriteLine(e);
                            }
                        }
                        else
                        {
                            Console.WriteLine("Drug already inserted");
                        }
                        break;

                    case 5:
                        Console.WriteLine("Enter the drug name: ");
                        drug = Console.ReadLine();                  

                        //check drug found or not
                        var foundDrug = dbContext.Drugs.Where(t => t.DrugName == drug).FirstOrDefault();
                        if(foundDrug == null)
                        {
                            Console.WriteLine("medicine is not found");
                        }
                        else
                        {
                            dbContext.Drugs.Remove(dbContext.Drugs.Single(t => t.DrugName == drug));
                            dbContext.SaveChanges();
                            Console.WriteLine("medicine successfully deleted");
                        }
                        break;

                    case 6:
                        Console.WriteLine("Department list");
                        var departmentList = dbContext.Departments;
                        foreach(var departments in departmentList)
                        {
                            Console.WriteLine("Department name : " + departments.DepartmentName);
                        }
                        break;

                    case 7:
                        Console.WriteLine("Doctor list");
                        var doctorList = dbContext.Doctors;
                        foreach(var doctor in doctorList)
                        {
                            Console.WriteLine("Doctor name : " + doctor.DoctorName);
                        }
                        break;
                       
                    case 8:
                        loop = false;
                        break;

                }
            }
        }
    }
}

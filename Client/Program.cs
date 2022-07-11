using Grpc.Net.Client;

namespace StudentsClient;

class Program
    {
        static async Task Main(string[] args)
        { 

            var channel = GrpcChannel.ForAddress("https://localhost:7003");
            
            string exit;
            do
            {
             Console.WriteLine("На сегодняшний день в базе есть эти люди:");
            await displayAllStudents(channel);
            Console.WriteLine("Выберите следующее действие:");
            Console.WriteLine("Добавить нового человека - enter  (NEW)");
            Console.WriteLine("Удалить человека из базы - enter (DEL)");
          
            string ans = Console.ReadLine();
            switch (ans)
            {
                case "NEW":
                    Console.WriteLine("Добавление нового человека:");
                    Console.WriteLine("Имя:");
                    string n_fn = Console.ReadLine();
                    Console.WriteLine("Фамилия:");
                    string n_ln = Console.ReadLine();
                    Console.WriteLine("Email:");
                    string n_em = Console.ReadLine();
                    
                    StudentFindModel newStudent = new StudentFindModel()
                    {
                        FirstName = n_fn,
                        LastName = n_ln,
                        Email = n_em,
                    };
                    await insertStudent(channel, newStudent);
                    break;
                
                case "DEL":
                    Console.WriteLine("Удаление человека с номером?:");
                    int d_id = Convert.ToInt32(Console.ReadLine());
                    await deleteStudent(channel, d_id);
                    break;
                case "EDIT":
                    Console.WriteLine("Изменение данных человека:");
                    Console.WriteLine("Номер студента?:");
                    int e_id = Convert.ToInt32(Console.ReadLine());
                    Console.WriteLine("Имя:");
                    string e_fn = Console.ReadLine();
                    Console.WriteLine("Фамилия:");
                    string e_ln = Console.ReadLine();
                    Console.WriteLine("Email:");
                    string e_em = Console.ReadLine();
                    
                    StudentFindModel updStudent = new StudentFindModel()
                    {
                        StudentId = e_id,
                        FirstName = e_fn,
                        LastName = e_ln,
                        Email = e_em,
                    };
                    await updateStudent(channel, updStudent);
                    break;
            }
            
            Console.WriteLine("Выход? Y/N");
            exit = Console.ReadLine();
            }
            while (exit == "N");
            Console.ReadLine();
        }

        static async Task findStudentById(GrpcChannel channel, int id)
        {
            var client = new RemoteStudent.RemoteStudentClient(channel);

            var input = new StudentDataModel { StudentId = id };
            var reply = await client.GetStudentInfoAsync(input);
            Console.WriteLine($"{reply.FirstName} {reply.LastName}");
        }

        static async Task insertStudent(GrpcChannel channel, StudentFindModel student)
        {
            var client = new RemoteStudent.RemoteStudentClient(channel);

            var reply = await client.InsertStudentAsync(student);
            Console.WriteLine(reply.Result);
        }

        static async Task updateStudent(GrpcChannel channel, StudentFindModel student)
        {
            var client = new RemoteStudent.RemoteStudentClient(channel);

            var reply = await client.UpdateStudentAsync(student);
            Console.WriteLine(reply.Result);
        }

        static async Task deleteStudent(GrpcChannel channel, int id)
        {
            var client = new RemoteStudent.RemoteStudentClient(channel);

            var input = new StudentDataModel { StudentId = id };
            var reply = await client.DeleteStudentAsync(input);
            Console.WriteLine(reply.Result);
        }

        static async Task displayAllStudents(GrpcChannel channel)
        {
            var client = new RemoteStudent.RemoteStudentClient(channel);

            var empty = new Empty();
            var list = await client.RetrieveAllStudentsAsync(empty);

            Console.WriteLine("--------------------------------------------");

            foreach (var item in list.Items)
            {
                Console.WriteLine($"{item.StudentId}: {item.FirstName} {item.LastName}");
            }
            Console.WriteLine("--------------------------------------------");
       }


    }
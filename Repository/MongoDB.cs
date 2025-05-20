using System;
using System.Windows;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;

namespace TCPChat_Assync.Repository
{
    public class MongoDB
    {
        private readonly IMongoCollection<Climas> climas_collection;
        private readonly IMongoCollection<Usuarios> usuarios_collection;

        static MongoDB()
        {
            var pack = new ConventionPack { new CamelCaseElementNameConvention() };
            ConventionRegistry.Register("elementNameConvention", pack, x => true);
        }

        public MongoDB()
        {
            var uri = "mongodb+srv://admin:123@cluster0.vgm081o.mongodb.net/AppChatClima?authSource=admin&retryWrites=true&w=majority&appName=Cluster0";
            var client = new MongoClient(uri);
            var database = client.GetDatabase("AppChatClima");
            climas_collection = database.GetCollection<Climas>("Climas");
            usuarios_collection = database.GetCollection<Usuarios>("Usuarios");
        }

        public void InsertUser(string nomeCompleto, string nomeUsuario, string senha)
        {
            try
            {
                var user = new Usuarios
                {
                    fullname = nomeCompleto,
                    username = nomeUsuario,
                    password = senha,
                    role = "user"
                };
                usuarios_collection.InsertOne(user);
            }
            catch (Exception ex)
            {
               CustomMessageBox.Show("Não foi possível cadastrar usuário\nTente novamente");
            }
        }

        public bool UserExists(string nomeUsuario)
        {
            nomeUsuario = nomeUsuario.Trim();
            var filtro = Builders<Usuarios>.Filter.Eq(u => u.username, nomeUsuario);
            return usuarios_collection.Find(filtro).Any();
        }

        public string LoginUser(string nomeUsuario, string senha)
        {
            nomeUsuario = nomeUsuario.Trim();
            senha = senha.Trim();

            try
            {
                var filtroUser = Builders<Usuarios>.Filter.Eq(u => u.username, nomeUsuario) &
                                 Builders<Usuarios>.Filter.Eq(u => u.password, senha);

                var user = usuarios_collection.Find(filtroUser).FirstOrDefault();

                if (user == null)
                {
                    CustomMessageBox.Show(
    "Usuário não encontrado.\n" +
    "Insira os dados informados corretamente ou realize o cadastro."
);
                }
                else if (user.role == "admin")
                {
                    return "admin";
                }
                else if (user.role == "user")
                {
                    return "user";
                }
            }
            catch (Exception ex)
            {
               CustomMessageBox.Show(
    "Usuário não encontrado.\n" +
    "Insira os dados informados corretamente ou realize o cadastro."
);
            }

            return null;
        }
    }

    public class Usuarios
    {
        public ObjectId id { get; set; }
        public string username { get; set; }
        public string fullname { get; set; }
        public string password { get; set; }
        public string role { get; set; }
    }

    public class Climas
    {
        public ObjectId Id { get; set; }
        public string Pais { get; set; }
        public string Cidade { get; set; }
        public double Temperatura { get; set; }
        public double ChuvaMM { get; set; }
        public string RiscoEnchente { get; set; }
    }
}

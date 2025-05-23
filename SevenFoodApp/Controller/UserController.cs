using SevenFoodApp.Model;
using SevenFoodApp.Repository;
using SevenFoodApp.Util;
using static SevenFoodApp.Util.Enums;

namespace SevenFoodApp.Controller
{
    internal class UserController
    {
        private UserRepository userRepository = new UserRepository(CONTEXT.USER);

        public UserResponseDTO Add(UserRequestDTO userDTO)
        {
            int id = this.GetNextId();
            string password = this.BuilderRandomPassword();
            User user = new User(id, userDTO.Name, password, userDTO.Type);
            if (userRepository.Insert(user))
            {
                UserResponseDTO responseDTO = new UserResponseDTO();
                responseDTO.Id = id;
                responseDTO.Name = userDTO.Name;
                responseDTO.Password = password;
                responseDTO.Type = userDTO.Type;

                return responseDTO;

            } else
            {
                UserResponseDTO responseDTO = new UserResponseDTO();
                responseDTO.Id = -1;

                return responseDTO;
            }
        }

        private string BuilderRandomPassword()
        {
            const string UPPER = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            const string LOWWER = "abcdefghijklmnopqrstuvwxyz";
            const string SPECIAL = "!@#$%&?><";
            const string DIGIT = "0123456789";

            var caracteres = new List<string>([UPPER, LOWWER, SPECIAL, DIGIT]);
            string password = "";
            Random index_randomic = new Random();

            for (int i = 0; i < 8; i++)
            {
                int index_type = index_randomic.Next(0, 4);
                int index_caractere = index_randomic.Next(0, caracteres[index_type].Length);
                char caracter_random = caracteres[index_type][index_caractere];
                password += caracter_random;
            }
            return password;
        }

        private bool IsValidPassWord(string password) => password.Length >= 8;

        private int GetNextId()
        {
            return Please.GetNextId();
        }

        private UserResponseDTO userResDTO = new UserResponseDTO();
        public List<UserResponseDTO>? getAll()
        {

            List<User> users = userRepository.GetAll();

            if (users != null && users.Count() > 0)
            {
                var usersDto = new List<UserResponseDTO>();

                foreach (var user in users)
                {
                    var userDto = this.castToDTO(user);
                    usersDto.Add(userDto);
                }
                return usersDto;
            }
            return null;
        }

        public Dictionary<string, string>? getById(int id)
        {
            User? user = userRepository.GetById(id);
            return user != null ? this.castObjectToDictionary(user) : null;
        }

        public bool remove(int id)
        {
            return userRepository.Delete(id);
        }

        internal bool update(int id, string name, string password, TYPE_USER type = TYPE_USER.Client)
        {
            User user = new User(id, name, password, type);
            return userRepository.Update(user);
        }

        public bool Loggin(string id, string password)
        {
            try
            {
                int _id = int.Parse(id);
                User user = userRepository.GetById(_id)!;
                return user != null && user.Password == password;
            }
            catch
            {
                return false;

            }

        }

        private UserResponseDTO castToDTO(User user)
        {
            UserResponseDTO responseDTO = new UserResponseDTO();
            responseDTO.Id = user.Id;
            responseDTO.Name = user.Name;
            responseDTO.Password = user.Password.Substring(0, 3) + "******";
            responseDTO.Type = user.Type;

            return responseDTO;
        }


        private Dictionary<string, string> castObjectToDictionary(User user)
        {
            var userString = new Dictionary<string, string>
                {
                    { "id", user.Id.ToString() },
                    { "name",  user.Name },
                    { "password", user.Password.Substring(0, 3) + "******" },
                    { "type", user.Type.Translate() },
                };
            return userString;
        }
    }
}

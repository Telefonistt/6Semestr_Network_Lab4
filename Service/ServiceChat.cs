using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;

namespace ChatService
{
    // ПРИМЕЧАНИЕ. Команду "Переименовать" в меню "Рефакторинг" можно использовать для одновременного изменения имени класса "Service1" в коде и файле конфигурации.
    [ServiceBehavior(InstanceContextMode =InstanceContextMode.Single)]
    public class ServiceChat : IServiceChat
    {
        List<ServerUser> users = new List<ServerUser>();
        int nextId = 1;
        public int Connect(string name)
        {
            var user = new ServerUser()
            {
                ID = nextId,
                Name = name,
                operationContext = OperationContext.Current
            };

            nextId++;
            SendMsg(" "+user.Name + "Подключился к чату!",0);

            users.Add(user);
            return user.ID;
        }

        public void Disconnect(int id)
        {
            ServerUser user = users.FirstOrDefault(i => i.ID == id);
            if(user!=null)
            {
                users.Remove(user);
                SendMsg(" "+user.Name + "Покинул чат",0);

            }
        }

        

        public void SendMsg(string msg, int id)
        {
            foreach (var item in users)
            {
                string answer = DateTime.Now.ToShortTimeString();


                ServerUser user = users.FirstOrDefault(i => i.ID == id);
                if (user != null)
                {
                    answer += ": " + user.Name+ "| ";
                }
                answer += msg;

                item.operationContext.GetCallbackChannel<IServerChatCallback>().MsgCallback(answer);
            }
        }
    }
}

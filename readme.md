# Harbor

프로그램 하나 만드는데 파일 저장 같은 것이 존재하여 오브젝트들을 관리하기 귀찮아지고, Serialize 작업 또한 고되기 쉽상입니다.  
**Harbor**는 사용자가 만드는 데이터를 저장하게 해줍니다.  
사용자는 Text Message, Voice data를 만드는데, 이것을 Private Directory에 넣어 관리할 수 있습니다.  
이것 뿐만이 아니라 원형으로 제공되는 ```Data class```는 원하는대로 사용 가능합니다.  

# Cargo
**Cargo**덕택에 필요한 정보를 실제 Directory로 옮기는데에 편리해지고, 코딩하기에 깔끔해집니다.  
Cargo에 User Log를 담을 만한 충분한 프로퍼티가 있는 커스텀 클래스를 작성하는 것도 좋습니다.  
사용자의 말들을 저장할 수 있습니다.
Cargo의 Unlock, Lock 기능으로 저장까지의 '변형'을 충분히 막을 수 있습니다.  


# Ship
Ship은 ```HttpShip```과 ```LocalShip```으로 나뉩니다. 이것들은 모은 Cargo들을 어느 서버에 Request 하거나, 실제 사용자 컴퓨터에 저장하는 작업을 수행합니다.  
LocalShip은 Constructor에서 지정한 다양한 Path에 관해 적재된 Cargo들을 저장합니다.  
한편, HttpShip은 마찬가지로 어떠한 주소로 xml serialized packet을 POST method로 전달합니다.

# Example
``` cs
using Harbor;
using Harbor.Ship;
using Harbor.Cargo;

namespace TestApp
{
    public class Program
    {
        static void Main(string[] args)
        {
            const string abs = @"C:\Users\me\Documents";
            LocalShip localShip = new LocalShip(
                new Dictionary<CargoType, string>() //Hidden folders
                {
                    {CargoType.GenericObject, $@"{abs}/generic_stuff"},
                    {CargoType.Text, $@"{abs}/txts_on_app"},
                    {CargoType.Voice, $@"{abs}/talks"},
                    {CargoType.Log, $@"{abs}/logdir"}
                },
                $@"{abs}\datalog" //Public folder
            );

            DataCargo logs = new DataCargo();

            logs.Load(new Data { Content = "code" });
            logs.Lock();

            localShip.LoadPublic(logs);
            localShip.PullAwayPublicData();

            foreach(var a in localShip.OpenPublicDataCargo(localShip.PublicDataSavepath, OpenFileFilter.All, OpenFileFilter.All))
            {
                a.UnLock();
                foreach(var b in a.GetDatas()) {
                    Console.WriteLine(b.Content);
                }
                
            }
            
            return;
        }
    }
}
```
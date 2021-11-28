# Harbor

���α׷� �ϳ� ����µ� ���� ���� ���� ���� �����Ͽ� ������Ʈ���� �����ϱ� ����������, Serialize �۾� ���� ��Ǳ� �����Դϴ�.  
**Harbor**�� ����ڰ� ����� �����͸� �����ϰ� ���ݴϴ�.  
����ڴ� Text Message, Voice data�� ����µ�, �̰��� Private Directory�� �־� ������ �� �ֽ��ϴ�.  
�̰� �Ӹ��� �ƴ϶� �������� �����Ǵ� ```Data class```�� ���ϴ´�� ��� �����մϴ�.  

# Cargo
**Cargo**���ÿ� �ʿ��� ������ ���� Directory�� �ű�µ��� ��������, �ڵ��ϱ⿡ ��������ϴ�.  
Cargo�� User Log�� ���� ���� ����� ������Ƽ�� �ִ� Ŀ���� Ŭ������ �ۼ��ϴ� �͵� �����ϴ�.  
������� ������ ������ �� �ֽ��ϴ�.
Cargo�� Unlock, Lock ������� ��������� '����'�� ����� ���� �� �ֽ��ϴ�.  


# Ship
Ship�� ```HttpShip```�� ```LocalShip```���� �����ϴ�. �̰͵��� ���� Cargo���� ��� ������ Request �ϰų�, ���� ����� ��ǻ�Ϳ� �����ϴ� �۾��� �����մϴ�.  
LocalShip�� Constructor���� ������ �پ��� Path�� ���� ����� Cargo���� �����մϴ�.  
����, HttpShip�� ���������� ��� �ּҷ� xml serialized packet�� POST method�� �����մϴ�.

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
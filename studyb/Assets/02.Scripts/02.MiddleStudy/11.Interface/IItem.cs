public interface IItem
{
    void Use(); 
}
// 인터페이스는 내부구현을 가질 수 없다 
// IItem 인터페이스를 상속받는 아이들은 무조건 Use라는 함수를 사용해야한다
// Use함수 안의 내용물들은 각자 알아서 짜도 상관이없다 이걸 다형성이라고 한다.
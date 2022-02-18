using Handler.Segments.Primitive;

namespace Handler.Segments
{
    /**
     * <summary>
     * 게임 진행의 기본 단위입니다. 이 Segment는 행동을 수행합니다. 이 Segment를 상속받으면서 행동을 customize할 수 있습니다.
     * 자세한 내용은 <see cref="CompiledNode"/>를 확인하십시오.
     * </summary>
     */
    public abstract class Segment : CompiledNode
    {
    }
}
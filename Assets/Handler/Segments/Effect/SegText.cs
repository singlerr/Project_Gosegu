using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using ScriptEngine.Elements;
using Handler.FlowContext;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Converters;

namespace Handler.Segments.Effect
{
    public class SegText : Segment
    {
        // input Dictionary (args[0])
        private Dictionary<String, Node> dialogue;
        // args: args[0]: dialogue dictionary<Speaker, text>
        public override SegmentResponse Execute(Context ctx, Collection<object> args)
        {
            // 교체 예정
            SegmentResponse response = new SegmentResponse(null, SegmentResponseType.Continue);
            // parameter optimization
            // args에 담겨있는 모든 Text를 json으로 deserialize 한 후 Dictionary<String, Node>의 원형 format으로 재배치.
            // 해당 파일들을 dialogue local variable로 저장
            // Collection이기 때문에 foreach loop을 썻지만 설명해주시는 바에 따르면 아마 하나의 line 일듯
            
            dialogue = args[0] as Dictionary<String, Node>;//JsonConvert.DeserializeObject<Dictionary<String, Node>>(json);
            printText();
            return response;
        }

        public override SegmentResponse OnSuspend(Context ctx)
        {
            throw new NotImplementedException();
        }
        // print dialogue text to scene
        private void printText(){
            // list of speakers - prob 1 speaker
            List<String> speaker = new List<String>(dialogue.Keys);
            // 대사
            Node text = dialogue[speaker[0]];
            // pipelineHandler 에서의 현 Segment의 사용 방법과 LineNode(?) 사용방법에 따라 해당 text print 예정
            // 대사의 가독성 (화자별 다른 textcolor 혹은 캐릭터 이미지 삽입)등을 위한 Uniformity 필요
            if(speaker[0].Equals("extra")){ // 엑스트라

            }else if(speaker[0].Equals("gosegu_conv")){ // 고세구 대화

            }else if(speaker[0].Equals("gosegu_mono")){ // 고세구 독백

            }else if(speaker[0].Equals("roent")){ // 뢴트게늄
                
            } // 추가 캐릭터
            else{
                // not-existing speaker error
            }
        }
    }
}
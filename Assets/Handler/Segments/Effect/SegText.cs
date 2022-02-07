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
        private Collection<Dictionary<String, Node>> dialogue;
        //args: Collection of Dictionary<string, Node>
        public override SegmentResponse Execute(Context ctx, Collection<object> args)
        {
            SegmentResponse response = new SegmentResponse(null, SegmentResponseType.Continue);
            // parameter optimization
            // args에 담겨있는 모든 Text를 json으로 deserialize 한 후 Dictionary<String, Node>의 원형 format으로 재배치.
            // 해당 파일들을 dialogue local variable로 저장
            foreach (object line in args){
                var json = JsonConvert.SerializeObject(line);
                Dictionary<String, Node> dictionary = JsonConvert.DeserializeObject<Dictionary<String, Node>>(json);
                dialogue.Add(dictionary);
            }
            printText();
            // 교체 예정
            return response;
        }

        public override SegmentResponse OnSuspend(Context ctx)
        {
            throw new NotImplementedException();
        }
        private void printText(){
            // pipelineHandler 에서의 현 Segment의 사용 방법에 따라 해당 function edit 예정
        }
    }
}
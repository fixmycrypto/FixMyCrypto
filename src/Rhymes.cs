using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace FixMyCrypto {
    class Rhymes {
        private static Dictionary<string, dynamic> rhymeWords = null;

        public Rhymes() {
            if (rhymeWords == null) {
                dynamic words = JsonConvert.DeserializeObject(json);
                rhymeWords = new Dictionary<string, dynamic>();
                foreach (var w in words) {
                    rhymeWords[(string)w.word] = w;
                }
            }
        }

        public List<string> GetWords(string word) {
            if (!rhymeWords.ContainsKey(word)) return new List<string>();

            var token = rhymeWords[word];

            List<string> rhymes = token.rhymes.ToObject<List<string>>();
            List<string> soundslike = token.soundsLike.ToObject<List<string>>();

            var l = new List<string>();
            l.AddRange(rhymes);
            l.AddRange(soundslike);
            
            return l;
        }

        private static string json = 
@"
[
  {
    ""word"": ""abandon"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""opinion""
    ]
  },
  {
    ""word"": ""ability"",
    ""rhymes"": [
      ""utility""
    ],
    ""soundsLike"": [
      ""apology""
    ]
  },
  {
    ""word"": ""able"",
    ""rhymes"": [
      ""table"",
      ""label"",
      ""enable"",
      ""stable"",
      ""cable"",
      ""unable""
    ],
    ""soundsLike"": [
      ""table"",
      ""label"",
      ""cable"",
      ""apple""
    ]
  },
  {
    ""word"": ""about"",
    ""rhymes"": [
      ""route"",
      ""scout""
    ],
    ""soundsLike"": [
      ""debate""
    ]
  },
  {
    ""word"": ""above"",
    ""rhymes"": [
      ""love"",
      ""dove"",
      ""shove"",
      ""glove""
    ],
    ""soundsLike"": [
      ""about"",
      ""approve""
    ]
  },
  {
    ""word"": ""absent"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""asset"",
      ""acid""
    ]
  },
  {
    ""word"": ""absorb"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""appear"",
      ""observe""
    ]
  },
  {
    ""word"": ""abstract"",
    ""rhymes"": [
      ""act"",
      ""impact"",
      ""enact"",
      ""intact"",
      ""exact"",
      ""attract"",
      ""pact""
    ],
    ""soundsLike"": [
      ""attract""
    ]
  },
  {
    ""word"": ""absurd"",
    ""rhymes"": [
      ""bird"",
      ""word""
    ],
    ""soundsLike"": [
      ""apart"",
      ""upset"",
      ""observe"",
      ""cupboard""
    ]
  },
  {
    ""word"": ""abuse"",
    ""rhymes"": [
      ""choose"",
      ""use"",
      ""produce"",
      ""goose"",
      ""news"",
      ""juice"",
      ""reduce"",
      ""refuse"",
      ""cruise"",
      ""excuse"",
      ""accuse""
    ],
    ""soundsLike"": []
  },
  {
    ""word"": ""access"",
    ""rhymes"": [
      ""process"",
      ""address"",
      ""express"",
      ""success"",
      ""dress"",
      ""guess"",
      ""excess"",
      ""bless""
    ],
    ""soundsLike"": [
      ""axis"",
      ""excess"",
      ""success""
    ]
  },
  {
    ""word"": ""accident"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""extend""
    ]
  },
  {
    ""word"": ""account"",
    ""rhymes"": [
      ""amount""
    ],
    ""soundsLike"": [
      ""again"",
      ""amount"",
      ""icon""
    ]
  },
  {
    ""word"": ""accuse"",
    ""rhymes"": [
      ""choose"",
      ""abuse"",
      ""news"",
      ""refuse"",
      ""cruise"",
      ""excuse""
    ],
    ""soundsLike"": [
      ""abuse"",
      ""across"",
      ""acquire""
    ]
  },
  {
    ""word"": ""achieve"",
    ""rhymes"": [
      ""leave"",
      ""naive"",
      ""believe"",
      ""receive""
    ],
    ""soundsLike"": [
      ""receive""
    ]
  },
  {
    ""word"": ""acid"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""asset""
    ]
  },
  {
    ""word"": ""acoustic"",
    ""rhymes"": [],
    ""soundsLike"": []
  },
  {
    ""word"": ""acquire"",
    ""rhymes"": [
      ""fire"",
      ""wire"",
      ""require"",
      ""hire"",
      ""entire"",
      ""liar"",
      ""retire"",
      ""buyer""
    ],
    ""soundsLike"": [
      ""aware"",
      ""require"",
      ""square"",
      ""occur"",
      ""accuse""
    ]
  },
  {
    ""word"": ""across"",
    ""rhymes"": [
      ""cross"",
      ""sauce"",
      ""boss"",
      ""toss""
    ],
    ""soundsLike"": [
      ""cross"",
      ""agree"",
      ""occur"",
      ""erase"",
      ""increase"",
      ""decrease"",
      ""accuse""
    ]
  },
  {
    ""word"": ""act"",
    ""rhymes"": [
      ""impact"",
      ""abstract"",
      ""enact"",
      ""intact"",
      ""exact"",
      ""attract"",
      ""pact""
    ],
    ""soundsLike"": [
      ""actor""
    ]
  },
  {
    ""word"": ""action"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""auction"",
      ""actual"",
      ""ocean"",
      ""fiction"",
      ""section""
    ]
  },
  {
    ""word"": ""actor"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""act""
    ]
  },
  {
    ""word"": ""actress"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""address"",
      ""actor"",
      ""cactus"",
      ""across""
    ]
  },
  {
    ""word"": ""actual"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""casual"",
      ""equal"",
      ""usual"",
      ""visual"",
      ""animal""
    ]
  },
  {
    ""word"": ""adapt"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""edit"",
      ""audit""
    ]
  },
  {
    ""word"": ""add"",
    ""rhymes"": [
      ""sad"",
      ""mad"",
      ""glad"",
      ""dad""
    ],
    ""soundsLike"": []
  },
  {
    ""word"": ""addict"",
    ""rhymes"": [
      ""perfect"",
      ""predict"",
      ""inflict""
    ],
    ""soundsLike"": [
      ""audit"",
      ""adult"",
      ""edit"",
      ""attack"",
      ""admit"",
      ""attract""
    ]
  },
  {
    ""word"": ""address"",
    ""rhymes"": [
      ""process"",
      ""access"",
      ""express"",
      ""success"",
      ""dress"",
      ""guess"",
      ""excess"",
      ""bless""
    ],
    ""soundsLike"": [
      ""actress"",
      ""advice""
    ]
  },
  {
    ""word"": ""adjust"",
    ""rhymes"": [
      ""trust"",
      ""robust"",
      ""just"",
      ""dust"",
      ""must""
    ],
    ""soundsLike"": [
      ""august"",
      ""assist"",
      ""just"",
      ""agent""
    ]
  },
  {
    ""word"": ""admit"",
    ""rhymes"": [
      ""fit"",
      ""split"",
      ""grit"",
      ""quit"",
      ""kit"",
      ""submit"",
      ""permit"",
      ""omit""
    ],
    ""soundsLike"": [
      ""audit"",
      ""omit"",
      ""edit"",
      ""addict"",
      ""adult""
    ]
  },
  {
    ""word"": ""adult"",
    ""rhymes"": [
      ""result""
    ],
    ""soundsLike"": [
      ""edit"",
      ""idle"",
      ""addict"",
      ""admit"",
      ""assault"",
      ""audit""
    ]
  },
  {
    ""word"": ""advance"",
    ""rhymes"": [
      ""enhance"",
      ""dance"",
      ""glance"",
      ""romance""
    ],
    ""soundsLike"": [
      ""advice"",
      ""oven"",
      ""address""
    ]
  },
  {
    ""word"": ""advice"",
    ""rhymes"": [
      ""ice"",
      ""device"",
      ""nice"",
      ""price"",
      ""rice"",
      ""dice"",
      ""spice"",
      ""slice"",
      ""twice""
    ],
    ""soundsLike"": [
      ""address"",
      ""advance"",
      ""device""
    ]
  },
  {
    ""word"": ""aerobic"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""arctic""
    ]
  },
  {
    ""word"": ""affair"",
    ""rhymes"": [
      ""air"",
      ""chair"",
      ""wear"",
      ""square"",
      ""share"",
      ""despair"",
      ""there"",
      ""hair"",
      ""spare"",
      ""aware"",
      ""pair"",
      ""where"",
      ""rare"",
      ""swear"",
      ""pear"",
      ""repair"",
      ""prepare"",
      ""glare"",
      ""scare"",
      ""warfare"",
      ""unfair"",
      ""unaware""
    ],
    ""soundsLike"": [
      ""unfair"",
      ""aware"",
      ""sphere"",
      ""afford"",
      ""offer"",
      ""appear""
    ]
  },
  {
    ""word"": ""afford"",
    ""rhymes"": [
      ""board"",
      ""record"",
      ""toward"",
      ""reward"",
      ""sword""
    ],
    ""soundsLike"": [
      ""affair"",
      ""afraid"",
      ""effort""
    ]
  },
  {
    ""word"": ""afraid"",
    ""rhymes"": [
      ""trade"",
      ""blade"",
      ""parade"",
      ""fade"",
      ""decade"",
      ""upgrade"",
      ""maid""
    ],
    ""soundsLike"": [
      ""effort"",
      ""afford"",
      ""erode""
    ]
  },
  {
    ""word"": ""again"",
    ""rhymes"": [
      ""train"",
      ""domain"",
      ""pen"",
      ""sustain"",
      ""then"",
      ""rain"",
      ""grain"",
      ""obtain"",
      ""brain"",
      ""gain"",
      ""explain"",
      ""hen"",
      ""main"",
      ""when"",
      ""crane"",
      ""insane"",
      ""remain"",
      ""ten""
    ],
    ""soundsLike"": []
  },
  {
    ""word"": ""age"",
    ""rhymes"": [
      ""gauge"",
      ""engage"",
      ""stage"",
      ""page"",
      ""cage"",
      ""wage""
    ],
    ""soundsLike"": [
      ""edge"",
      ""cage"",
      ""gauge""
    ]
  },
  {
    ""word"": ""agent"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""patient"",
      ""ancient"",
      ""adjust""
    ]
  },
  {
    ""word"": ""agree"",
    ""rhymes"": [
      ""tree"",
      ""sea"",
      ""key"",
      ""flee"",
      ""degree"",
      ""debris"",
      ""fee"",
      ""ski"",
      ""three"",
      ""pony"",
      ""knee""
    ],
    ""soundsLike"": [
      ""degree""
    ]
  },
  {
    ""word"": ""ahead"",
    ""rhymes"": [
      ""head"",
      ""spread"",
      ""shed"",
      ""bread""
    ],
    ""soundsLike"": []
  },
  {
    ""word"": ""aim"",
    ""rhymes"": [
      ""game"",
      ""name"",
      ""frame"",
      ""claim"",
      ""same"",
      ""blame"",
      ""flame"",
      ""fame""
    ],
    ""soundsLike"": []
  },
  {
    ""word"": ""air"",
    ""rhymes"": [
      ""chair"",
      ""wear"",
      ""square"",
      ""share"",
      ""despair"",
      ""there"",
      ""hair"",
      ""spare"",
      ""aware"",
      ""pair"",
      ""where"",
      ""rare"",
      ""swear"",
      ""pear"",
      ""affair"",
      ""repair"",
      ""prepare"",
      ""glare"",
      ""scare"",
      ""warfare"",
      ""unfair"",
      ""unaware""
    ],
    ""soundsLike"": [
      ""hour""
    ]
  },
  {
    ""word"": ""airport"",
    ""rhymes"": [
      ""short"",
      ""report"",
      ""sport"",
      ""sort""
    ],
    ""soundsLike"": [
      ""apart"",
      ""sport"",
      ""report""
    ]
  },
  {
    ""word"": ""aisle"",
    ""rhymes"": [
      ""style"",
      ""file"",
      ""trial"",
      ""smile"",
      ""exile"",
      ""denial"",
      ""dial""
    ],
    ""soundsLike"": [
      ""all"",
      ""ill"",
      ""oil""
    ]
  },
  {
    ""word"": ""alarm"",
    ""rhymes"": [
      ""farm"",
      ""arm""
    ],
    ""soundsLike"": [
      ""alone""
    ]
  },
  {
    ""word"": ""album"",
    ""rhymes"": [],
    ""soundsLike"": []
  },
  {
    ""word"": ""alcohol"",
    ""rhymes"": [
      ""all"",
      ""ball"",
      ""call"",
      ""fall"",
      ""wall"",
      ""recall"",
      ""small"",
      ""doll"",
      ""install"",
      ""crawl""
    ],
    ""soundsLike"": [
      ""ankle""
    ]
  },
  {
    ""word"": ""alert"",
    ""rhymes"": [
      ""desert"",
      ""hurt"",
      ""skirt"",
      ""concert"",
      ""divert"",
      ""dirt""
    ],
    ""soundsLike"": [
      ""elite""
    ]
  },
  {
    ""word"": ""alien"",
    ""rhymes"": [],
    ""soundsLike"": []
  },
  {
    ""word"": ""all"",
    ""rhymes"": [
      ""ball"",
      ""call"",
      ""fall"",
      ""wall"",
      ""recall"",
      ""small"",
      ""alcohol"",
      ""doll"",
      ""install"",
      ""crawl""
    ],
    ""soundsLike"": [
      ""ill"",
      ""oil"",
      ""aisle""
    ]
  },
  {
    ""word"": ""alley"",
    ""rhymes"": [
      ""rally"",
      ""valley""
    ],
    ""soundsLike"": []
  },
  {
    ""word"": ""allow"",
    ""rhymes"": [
      ""now"",
      ""eyebrow""
    ],
    ""soundsLike"": [
      ""alley""
    ]
  },
  {
    ""word"": ""almost"",
    ""rhymes"": [
      ""post"",
      ""host"",
      ""coast"",
      ""roast"",
      ""ghost"",
      ""toast""
    ],
    ""soundsLike"": [
      ""illness""
    ]
  },
  {
    ""word"": ""alone"",
    ""rhymes"": [
      ""bone"",
      ""zone"",
      ""stone"",
      ""tone"",
      ""loan"",
      ""phone"",
      ""own"",
      ""ozone"",
      ""unknown""
    ],
    ""soundsLike"": []
  },
  {
    ""word"": ""alpha"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""alley""
    ]
  },
  {
    ""word"": ""already"",
    ""rhymes"": [
      ""ready""
    ],
    ""soundsLike"": []
  },
  {
    ""word"": ""also"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""else""
    ]
  },
  {
    ""word"": ""alter"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""elder"",
      ""outer""
    ]
  },
  {
    ""word"": ""always"",
    ""rhymes"": [
      ""raise"",
      ""phrase"",
      ""praise"",
      ""gaze"",
      ""maze""
    ],
    ""soundsLike"": []
  },
  {
    ""word"": ""amateur"",
    ""rhymes"": [
      ""transfer"",
      ""occur"",
      ""prefer"",
      ""blur""
    ],
    ""soundsLike"": [
      ""actor""
    ]
  },
  {
    ""word"": ""amazing"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""among""
    ]
  },
  {
    ""word"": ""among"",
    ""rhymes"": [
      ""tongue"",
      ""young""
    ],
    ""soundsLike"": []
  },
  {
    ""word"": ""amount"",
    ""rhymes"": [
      ""account""
    ],
    ""soundsLike"": [
      ""cement"",
      ""account"",
      ""among"",
      ""announce""
    ]
  },
  {
    ""word"": ""amused"",
    ""rhymes"": [
      ""used""
    ],
    ""soundsLike"": []
  },
  {
    ""word"": ""analyst"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""enlist""
    ]
  },
  {
    ""word"": ""anchor"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""anger"",
      ""angry""
    ]
  },
  {
    ""word"": ""ancient"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""agent"",
      ""engine"",
      ""patient"",
      ""payment""
    ]
  },
  {
    ""word"": ""anger"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""anchor"",
      ""angry"",
      ""eager""
    ]
  },
  {
    ""word"": ""angle"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""ankle"",
      ""uncle"",
      ""eagle""
    ]
  },
  {
    ""word"": ""angry"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""anger"",
      ""anchor"",
      ""kangaroo"",
      ""hungry"",
      ""agree""
    ]
  },
  {
    ""word"": ""animal"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""annual""
    ]
  },
  {
    ""word"": ""ankle"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""angle"",
      ""uncle""
    ]
  },
  {
    ""word"": ""announce"",
    ""rhymes"": [
      ""bounce""
    ],
    ""soundsLike"": [
      ""immense"",
      ""amount"",
      ""enhance""
    ]
  },
  {
    ""word"": ""annual"",
    ""rhymes"": [
      ""manual""
    ],
    ""soundsLike"": [
      ""manual"",
      ""animal"",
      ""angle"",
      ""ankle""
    ]
  },
  {
    ""word"": ""another"",
    ""rhymes"": [
      ""other"",
      ""rather"",
      ""mother"",
      ""brother""
    ],
    ""soundsLike"": [
      ""under"",
      ""other"",
      ""unaware"",
      ""mother"",
      ""rather"",
      ""amateur"",
      ""aware""
    ]
  },
  {
    ""word"": ""answer"",
    ""rhymes"": [],
    ""soundsLike"": []
  },
  {
    ""word"": ""antenna"",
    ""rhymes"": [],
    ""soundsLike"": []
  },
  {
    ""word"": ""antique"",
    ""rhymes"": [
      ""seek"",
      ""unique"",
      ""bleak"",
      ""speak"",
      ""creek""
    ],
    ""soundsLike"": []
  },
  {
    ""word"": ""anxiety"",
    ""rhymes"": [],
    ""soundsLike"": []
  },
  {
    ""word"": ""any"",
    ""rhymes"": [
      ""twenty""
    ],
    ""soundsLike"": [
      ""end""
    ]
  },
  {
    ""word"": ""apart"",
    ""rhymes"": [
      ""art"",
      ""smart"",
      ""heart"",
      ""start"",
      ""cart"",
      ""depart""
    ],
    ""soundsLike"": [
      ""sport"",
      ""appear"",
      ""depart"",
      ""about"",
      ""report"",
      ""airport"",
      ""absurd"",
      ""alert""
    ]
  },
  {
    ""word"": ""apology"",
    ""rhymes"": [
      ""ecology"",
      ""biology""
    ],
    ""soundsLike"": [
      ""ability"",
      ""ecology"",
      ""biology""
    ]
  },
  {
    ""word"": ""appear"",
    ""rhymes"": [
      ""year"",
      ""deer"",
      ""pioneer"",
      ""near"",
      ""sphere""
    ],
    ""soundsLike"": [
      ""upper"",
      ""aware"",
      ""apart"",
      ""spare"",
      ""oppose""
    ]
  },
  {
    ""word"": ""apple"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""able""
    ]
  },
  {
    ""word"": ""approve"",
    ""rhymes"": [
      ""move"",
      ""improve"",
      ""remove""
    ],
    ""soundsLike"": [
      ""improve"",
      ""above"",
      ""upper"",
      ""opera""
    ]
  },
  {
    ""word"": ""april"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""able"",
      ""apple"",
      ""opera"",
      ""maple""
    ]
  },
  {
    ""word"": ""arch"",
    ""rhymes"": [
      ""march""
    ],
    ""soundsLike"": [
      ""art"",
      ""march""
    ]
  },
  {
    ""word"": ""arctic"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""critic""
    ]
  },
  {
    ""word"": ""area"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""era""
    ]
  },
  {
    ""word"": ""arena"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""renew""
    ]
  },
  {
    ""word"": ""argue"",
    ""rhymes"": [],
    ""soundsLike"": []
  },
  {
    ""word"": ""arm"",
    ""rhymes"": [
      ""farm"",
      ""alarm""
    ],
    ""soundsLike"": [
      ""farm"",
      ""army"",
      ""armor"",
      ""armed""
    ]
  },
  {
    ""word"": ""armed"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""arm"",
      ""army"",
      ""art"",
      ""armor""
    ]
  },
  {
    ""word"": ""armor"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""arm"",
      ""army"",
      ""armed""
    ]
  },
  {
    ""word"": ""army"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""arm"",
      ""armor"",
      ""armed""
    ]
  },
  {
    ""word"": ""around"",
    ""rhymes"": [
      ""round"",
      ""sound"",
      ""brown"",
      ""town"",
      ""found"",
      ""frown"",
      ""gown"",
      ""surround"",
      ""clown""
    ],
    ""soundsLike"": [
      ""round"",
      ""rent"",
      ""surround""
    ]
  },
  {
    ""word"": ""arrange"",
    ""rhymes"": [
      ""change"",
      ""range"",
      ""exchange""
    ],
    ""soundsLike"": [
      ""range"",
      ""rain"",
      ""ranch"",
      ""orange"",
      ""around"",
      ""arena"",
      ""fringe""
    ]
  },
  {
    ""word"": ""arrest"",
    ""rhymes"": [
      ""test"",
      ""best"",
      ""suggest"",
      ""nest"",
      ""chest"",
      ""west"",
      ""invest""
    ],
    ""soundsLike"": [
      ""wrist"",
      ""roast"",
      ""west""
    ]
  },
  {
    ""word"": ""arrive"",
    ""rhymes"": [
      ""drive"",
      ""live"",
      ""thrive"",
      ""derive""
    ],
    ""soundsLike"": [
      ""drive"",
      ""thrive"",
      ""derive"",
      ""brave""
    ]
  },
  {
    ""word"": ""arrow"",
    ""rhymes"": [
      ""narrow""
    ],
    ""soundsLike"": [
      ""narrow""
    ]
  },
  {
    ""word"": ""art"",
    ""rhymes"": [
      ""smart"",
      ""heart"",
      ""start"",
      ""apart"",
      ""cart"",
      ""depart""
    ],
    ""soundsLike"": [
      ""cart"",
      ""arch""
    ]
  },
  {
    ""word"": ""artefact"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""arctic"",
      ""perfect"",
      ""protect"",
      ""artist""
    ]
  },
  {
    ""word"": ""artist"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""trust""
    ]
  },
  {
    ""word"": ""artwork"",
    ""rhymes"": [
      ""work"",
      ""network"",
      ""clerk""
    ],
    ""soundsLike"": [
      ""arctic"",
      ""network""
    ]
  },
  {
    ""word"": ""ask"",
    ""rhymes"": [
      ""mask"",
      ""task""
    ],
    ""soundsLike"": [
      ""task"",
      ""mask""
    ]
  },
  {
    ""word"": ""aspect"",
    ""rhymes"": [
      ""project"",
      ""subject"",
      ""object"",
      ""perfect"",
      ""reflect"",
      ""direct"",
      ""connect"",
      ""expect"",
      ""correct"",
      ""neglect"",
      ""suspect"",
      ""collect"",
      ""select"",
      ""protect"",
      ""reject"",
      ""detect"",
      ""insect"",
      ""inject""
    ],
    ""soundsLike"": [
      ""expect"",
      ""suspect"",
      ""asset"",
      ""impact"",
      ""insect""
    ]
  },
  {
    ""word"": ""assault"",
    ""rhymes"": [
      ""salt"",
      ""vault"",
      ""fault""
    ],
    ""soundsLike"": [
      ""salt"",
      ""adult""
    ]
  },
  {
    ""word"": ""asset"",
    ""rhymes"": [
      ""net"",
      ""upset"",
      ""wet"",
      ""forget"",
      ""regret"",
      ""pet"",
      ""sunset""
    ],
    ""soundsLike"": [
      ""acid""
    ]
  },
  {
    ""word"": ""assist"",
    ""rhymes"": [
      ""list"",
      ""just"",
      ""twist"",
      ""exist"",
      ""resist"",
      ""enlist"",
      ""wrist""
    ],
    ""soundsLike"": [
      ""adjust"",
      ""asset""
    ]
  },
  {
    ""word"": ""assume"",
    ""rhymes"": [
      ""room"",
      ""broom"",
      ""gloom""
    ],
    ""soundsLike"": [
      ""awesome""
    ]
  },
  {
    ""word"": ""asthma"",
    ""rhymes"": [],
    ""soundsLike"": []
  },
  {
    ""word"": ""athlete"",
    ""rhymes"": [
      ""seat"",
      ""treat"",
      ""street"",
      ""elite"",
      ""sweet"",
      ""meat"",
      ""retreat"",
      ""wheat"",
      ""repeat""
    ],
    ""soundsLike"": [
      ""elite"",
      ""alley""
    ]
  },
  {
    ""word"": ""atom"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""autumn"",
      ""item""
    ]
  },
  {
    ""word"": ""attack"",
    ""rhymes"": [
      ""black"",
      ""track"",
      ""crack"",
      ""rack"",
      ""snack""
    ],
    ""soundsLike"": [
      ""awake"",
      ""steak""
    ]
  },
  {
    ""word"": ""attend"",
    ""rhymes"": [
      ""end"",
      ""friend"",
      ""lend"",
      ""trend"",
      ""extend"",
      ""spend"",
      ""depend"",
      ""weekend""
    ],
    ""soundsLike"": [
      ""stand"",
      ""ahead""
    ]
  },
  {
    ""word"": ""attitude"",
    ""rhymes"": [
      ""food"",
      ""rude"",
      ""include"",
      ""exclude""
    ],
    ""soundsLike"": []
  },
  {
    ""word"": ""attract"",
    ""rhymes"": [
      ""act"",
      ""impact"",
      ""abstract"",
      ""enact"",
      ""intact"",
      ""exact"",
      ""pact""
    ],
    ""soundsLike"": [
      ""attack"",
      ""addict"",
      ""intact""
    ]
  },
  {
    ""word"": ""auction"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""action"",
      ""ocean"",
      ""fiction"",
      ""section""
    ]
  },
  {
    ""word"": ""audit"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""edit"",
      ""admit"",
      ""addict""
    ]
  },
  {
    ""word"": ""august"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""adjust"",
      ""exist""
    ]
  },
  {
    ""word"": ""aunt"",
    ""rhymes"": [
      ""grant"",
      ""want""
    ],
    ""soundsLike"": []
  },
  {
    ""word"": ""author"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""other"",
      ""either"",
      ""offer"",
      ""father"",
      ""hour"",
      ""outer""
    ]
  },
  {
    ""word"": ""auto"",
    ""rhymes"": [],
    ""soundsLike"": []
  },
  {
    ""word"": ""autumn"",
    ""rhymes"": [
      ""bottom""
    ],
    ""soundsLike"": [
      ""item"",
      ""atom"",
      ""awesome"",
      ""bottom"",
      ""cotton"",
      ""auto""
    ]
  },
  {
    ""word"": ""average"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""bridge"",
      ""marriage"",
      ""ivory""
    ]
  },
  {
    ""word"": ""avocado"",
    ""rhymes"": [],
    ""soundsLike"": []
  },
  {
    ""word"": ""avoid"",
    ""rhymes"": [
      ""void""
    ],
    ""soundsLike"": [
      ""ahead"",
      ""divide""
    ]
  },
  {
    ""word"": ""awake"",
    ""rhymes"": [
      ""cake"",
      ""make"",
      ""snake"",
      ""mistake"",
      ""lake"",
      ""steak""
    ],
    ""soundsLike"": [
      ""away"",
      ""attack"",
      ""quick""
    ]
  },
  {
    ""word"": ""aware"",
    ""rhymes"": [
      ""air"",
      ""chair"",
      ""wear"",
      ""square"",
      ""share"",
      ""despair"",
      ""there"",
      ""hair"",
      ""spare"",
      ""pair"",
      ""where"",
      ""rare"",
      ""swear"",
      ""pear"",
      ""affair"",
      ""repair"",
      ""prepare"",
      ""glare"",
      ""scare"",
      ""warfare"",
      ""unfair"",
      ""unaware""
    ],
    ""soundsLike"": [
      ""where"",
      ""swear"",
      ""appear"",
      ""away"",
      ""affair"",
      ""acquire""
    ]
  },
  {
    ""word"": ""away"",
    ""rhymes"": [
      ""day"",
      ""way"",
      ""play"",
      ""say"",
      ""survey"",
      ""display"",
      ""stay"",
      ""delay"",
      ""holiday"",
      ""essay"",
      ""clay"",
      ""betray"",
      ""spray"",
      ""they"",
      ""okay"",
      ""tray"",
      ""obey"",
      ""today""
    ],
    ""soundsLike"": [
      ""awake""
    ]
  },
  {
    ""word"": ""awesome"",
    ""rhymes"": [
      ""blossom""
    ],
    ""soundsLike"": [
      ""assume""
    ]
  },
  {
    ""word"": ""awful"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""evil"",
      ""aisle""
    ]
  },
  {
    ""word"": ""awkward"",
    ""rhymes"": [],
    ""soundsLike"": []
  },
  {
    ""word"": ""axis"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""access"",
      ""excess""
    ]
  },
  {
    ""word"": ""baby"",
    ""rhymes"": [],
    ""soundsLike"": []
  },
  {
    ""word"": ""bachelor"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""battle"",
      ""barrel""
    ]
  },
  {
    ""word"": ""bacon"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""become"",
      ""begin""
    ]
  },
  {
    ""word"": ""badge"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""bag"",
      ""beach"",
      ""page"",
      ""patch""
    ]
  },
  {
    ""word"": ""bag"",
    ""rhymes"": [
      ""flag"",
      ""tag""
    ],
    ""soundsLike"": [
      ""badge"",
      ""book"",
      ""bike""
    ]
  },
  {
    ""word"": ""balance"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""palace"",
      ""balcony""
    ]
  },
  {
    ""word"": ""balcony"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""balance"",
      ""bacon""
    ]
  },
  {
    ""word"": ""ball"",
    ""rhymes"": [
      ""all"",
      ""call"",
      ""fall"",
      ""wall"",
      ""recall"",
      ""small"",
      ""alcohol"",
      ""doll"",
      ""install"",
      ""crawl""
    ],
    ""soundsLike"": [
      ""boil""
    ]
  },
  {
    ""word"": ""bamboo"",
    ""rhymes"": [
      ""blue"",
      ""into"",
      ""you"",
      ""view"",
      ""review"",
      ""true"",
      ""two"",
      ""shoe"",
      ""crew"",
      ""tissue"",
      ""few"",
      ""glue"",
      ""undo"",
      ""tattoo"",
      ""renew"",
      ""canoe"",
      ""zoo""
    ],
    ""soundsLike"": [
      ""baby""
    ]
  },
  {
    ""word"": ""banana"",
    ""rhymes"": [
      ""piano""
    ],
    ""soundsLike"": [
      ""piano""
    ]
  },
  {
    ""word"": ""banner"",
    ""rhymes"": [],
    ""soundsLike"": []
  },
  {
    ""word"": ""bar"",
    ""rhymes"": [
      ""car"",
      ""jar"",
      ""radar"",
      ""guitar"",
      ""seminar"",
      ""jaguar"",
      ""cigar""
    ],
    ""soundsLike"": [
      ""buyer""
    ]
  },
  {
    ""word"": ""barely"",
    ""rhymes"": [],
    ""soundsLike"": []
  },
  {
    ""word"": ""bargain"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""begin""
    ]
  },
  {
    ""word"": ""barrel"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""battle""
    ]
  },
  {
    ""word"": ""base"",
    ""rhymes"": [
      ""case"",
      ""grace"",
      ""face"",
      ""place"",
      ""space"",
      ""race"",
      ""embrace"",
      ""chase"",
      ""replace"",
      ""erase""
    ],
    ""soundsLike"": [
      ""bus"",
      ""boss""
    ]
  },
  {
    ""word"": ""basic"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""base""
    ]
  },
  {
    ""word"": ""basket"",
    ""rhymes"": [],
    ""soundsLike"": []
  },
  {
    ""word"": ""battle"",
    ""rhymes"": [
      ""cattle""
    ],
    ""soundsLike"": [
      ""paddle"",
      ""barrel""
    ]
  },
  {
    ""word"": ""beach"",
    ""rhymes"": [
      ""teach""
    ],
    ""soundsLike"": [
      ""bean"",
      ""badge""
    ]
  },
  {
    ""word"": ""bean"",
    ""rhymes"": [
      ""mean"",
      ""green"",
      ""machine"",
      ""keen"",
      ""screen"",
      ""clean"",
      ""between"",
      ""scene"",
      ""marine""
    ],
    ""soundsLike"": [
      ""bone"",
      ""beach""
    ]
  },
  {
    ""word"": ""beauty"",
    ""rhymes"": [
      ""duty""
    ],
    ""soundsLike"": []
  },
  {
    ""word"": ""because"",
    ""rhymes"": [
      ""cause"",
      ""buzz"",
      ""pause""
    ],
    ""soundsLike"": []
  },
  {
    ""word"": ""become"",
    ""rhymes"": [
      ""come"",
      ""income"",
      ""thumb"",
      ""drum"",
      ""dumb""
    ],
    ""soundsLike"": [
      ""bacon"",
      ""begin"",
      ""because""
    ]
  },
  {
    ""word"": ""beef"",
    ""rhymes"": [
      ""leaf"",
      ""relief"",
      ""brief"",
      ""chief"",
      ""grief""
    ],
    ""soundsLike"": [
      ""brief""
    ]
  },
  {
    ""word"": ""before"",
    ""rhymes"": [
      ""door"",
      ""more"",
      ""core"",
      ""floor"",
      ""ignore"",
      ""dinosaur"",
      ""outdoor"",
      ""indoor""
    ],
    ""soundsLike"": []
  },
  {
    ""word"": ""begin"",
    ""rhymes"": [
      ""spin"",
      ""skin"",
      ""win"",
      ""when"",
      ""twin"",
      ""violin""
    ],
    ""soundsLike"": [
      ""become"",
      ""bacon"",
      ""bargain""
    ]
  },
  {
    ""word"": ""behave"",
    ""rhymes"": [
      ""wave"",
      ""brave"",
      ""save"",
      ""cave"",
      ""pave""
    ],
    ""soundsLike"": [
      ""believe"",
      ""brave""
    ]
  },
  {
    ""word"": ""behind"",
    ""rhymes"": [
      ""bind"",
      ""mind"",
      ""find"",
      ""kind"",
      ""blind"",
      ""remind""
    ],
    ""soundsLike"": [
      ""beyond"",
      ""blind"",
      ""bind""
    ]
  },
  {
    ""word"": ""believe"",
    ""rhymes"": [
      ""leave"",
      ""naive"",
      ""achieve"",
      ""receive""
    ],
    ""soundsLike"": [
      ""below"",
      ""behave""
    ]
  },
  {
    ""word"": ""below"",
    ""rhymes"": [
      ""know"",
      ""snow"",
      ""throw"",
      ""hello"",
      ""grow"",
      ""shadow"",
      ""potato"",
      ""toe"",
      ""glow"",
      ""slow"",
      ""tomato"",
      ""radio"",
      ""meadow"",
      ""tomorrow"",
      ""borrow"",
      ""photo"",
      ""tornado"",
      ""buffalo""
    ],
    ""soundsLike"": []
  },
  {
    ""word"": ""belt"",
    ""rhymes"": [
      ""melt""
    ],
    ""soundsLike"": [
      ""build"",
      ""below""
    ]
  },
  {
    ""word"": ""bench"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""punch""
    ]
  },
  {
    ""word"": ""benefit"",
    ""rhymes"": [],
    ""soundsLike"": []
  },
  {
    ""word"": ""best"",
    ""rhymes"": [
      ""test"",
      ""suggest"",
      ""nest"",
      ""chest"",
      ""west"",
      ""arrest"",
      ""invest""
    ],
    ""soundsLike"": [
      ""boost"",
      ""burst"",
      ""post""
    ]
  },
  {
    ""word"": ""betray"",
    ""rhymes"": [
      ""day"",
      ""away"",
      ""way"",
      ""play"",
      ""say"",
      ""survey"",
      ""display"",
      ""stay"",
      ""delay"",
      ""holiday"",
      ""essay"",
      ""clay"",
      ""spray"",
      ""they"",
      ""okay"",
      ""tray"",
      ""obey"",
      ""today""
    ],
    ""soundsLike"": [
      ""bitter"",
      ""better"",
      ""butter""
    ]
  },
  {
    ""word"": ""better"",
    ""rhymes"": [
      ""letter""
    ],
    ""soundsLike"": [
      ""butter"",
      ""bitter""
    ]
  },
  {
    ""word"": ""between"",
    ""rhymes"": [
      ""mean"",
      ""green"",
      ""bean"",
      ""machine"",
      ""keen"",
      ""screen"",
      ""clean"",
      ""scene"",
      ""marine""
    ],
    ""soundsLike"": []
  },
  {
    ""word"": ""beyond"",
    ""rhymes"": [
      ""pond""
    ],
    ""soundsLike"": [
      ""behind"",
      ""blind"",
      ""brand"",
      ""bind"",
      ""pond""
    ]
  },
  {
    ""word"": ""bicycle"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""basic""
    ]
  },
  {
    ""word"": ""bid"",
    ""rhymes"": [
      ""good"",
      ""grid"",
      ""kid""
    ],
    ""soundsLike"": [
      ""bird""
    ]
  },
  {
    ""word"": ""bike"",
    ""rhymes"": [
      ""strike"",
      ""like"",
      ""spike""
    ],
    ""soundsLike"": [
      ""book"",
      ""bag""
    ]
  },
  {
    ""word"": ""bind"",
    ""rhymes"": [
      ""mind"",
      ""find"",
      ""kind"",
      ""blind"",
      ""behind"",
      ""remind""
    ],
    ""soundsLike"": [
      ""blind"",
      ""pond""
    ]
  },
  {
    ""word"": ""biology"",
    ""rhymes"": [
      ""ecology"",
      ""apology""
    ],
    ""soundsLike"": [
      ""apology"",
      ""ecology""
    ]
  },
  {
    ""word"": ""bird"",
    ""rhymes"": [
      ""word"",
      ""absurd""
    ],
    ""soundsLike"": [
      ""board"",
      ""bread"",
      ""bid""
    ]
  },
  {
    ""word"": ""birth"",
    ""rhymes"": [
      ""earth"",
      ""worth""
    ],
    ""soundsLike"": []
  },
  {
    ""word"": ""bitter"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""better"",
      ""butter"",
      ""betray""
    ]
  },
  {
    ""word"": ""black"",
    ""rhymes"": [
      ""track"",
      ""attack"",
      ""crack"",
      ""rack"",
      ""snack""
    ],
    ""soundsLike"": [
      ""bleak"",
      ""pluck"",
      ""bag""
    ]
  },
  {
    ""word"": ""blade"",
    ""rhymes"": [
      ""trade"",
      ""parade"",
      ""fade"",
      ""afraid"",
      ""decade"",
      ""upgrade"",
      ""maid""
    ],
    ""soundsLike"": [
      ""blood"",
      ""plate"",
      ""bid""
    ]
  },
  {
    ""word"": ""blame"",
    ""rhymes"": [
      ""game"",
      ""name"",
      ""frame"",
      ""claim"",
      ""aim"",
      ""same"",
      ""flame"",
      ""fame""
    ],
    ""soundsLike"": []
  },
  {
    ""word"": ""blanket"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""planet""
    ]
  },
  {
    ""word"": ""blast"",
    ""rhymes"": [
      ""vast""
    ],
    ""soundsLike"": [
      ""best"",
      ""bless"",
      ""boost"",
      ""blouse""
    ]
  },
  {
    ""word"": ""bleak"",
    ""rhymes"": [
      ""seek"",
      ""unique"",
      ""speak"",
      ""creek"",
      ""antique""
    ],
    ""soundsLike"": [
      ""black"",
      ""pluck""
    ]
  },
  {
    ""word"": ""bless"",
    ""rhymes"": [
      ""process"",
      ""address"",
      ""access"",
      ""express"",
      ""success"",
      ""dress"",
      ""guess"",
      ""excess""
    ],
    ""soundsLike"": [
      ""blouse"",
      ""place"",
      ""bus"",
      ""blush""
    ]
  },
  {
    ""word"": ""blind"",
    ""rhymes"": [
      ""bind"",
      ""mind"",
      ""find"",
      ""kind"",
      ""behind"",
      ""remind""
    ],
    ""soundsLike"": [
      ""bind"",
      ""blood"",
      ""beyond"",
      ""blade""
    ]
  },
  {
    ""word"": ""blood"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""blade"",
      ""bid""
    ]
  },
  {
    ""word"": ""blossom"",
    ""rhymes"": [
      ""awesome""
    ],
    ""soundsLike"": []
  },
  {
    ""word"": ""blouse"",
    ""rhymes"": [
      ""mouse""
    ],
    ""soundsLike"": [
      ""bless"",
      ""place"",
      ""boss"",
      ""blast"",
      ""base""
    ]
  },
  {
    ""word"": ""blue"",
    ""rhymes"": [
      ""into"",
      ""you"",
      ""view"",
      ""review"",
      ""true"",
      ""two"",
      ""shoe"",
      ""crew"",
      ""tissue"",
      ""few"",
      ""glue"",
      ""bamboo"",
      ""undo"",
      ""tattoo"",
      ""renew"",
      ""canoe"",
      ""zoo""
    ],
    ""soundsLike"": []
  },
  {
    ""word"": ""blur"",
    ""rhymes"": [
      ""transfer"",
      ""occur"",
      ""amateur"",
      ""prefer""
    ],
    ""soundsLike"": [
      ""bar""
    ]
  },
  {
    ""word"": ""blush"",
    ""rhymes"": [
      ""flush"",
      ""crush"",
      ""brush"",
      ""slush""
    ],
    ""soundsLike"": [
      ""bless"",
      ""blood""
    ]
  },
  {
    ""word"": ""board"",
    ""rhymes"": [
      ""record"",
      ""afford"",
      ""toward"",
      ""reward"",
      ""sword""
    ],
    ""soundsLike"": [
      ""bird"",
      ""border"",
      ""bar""
    ]
  },
  {
    ""word"": ""boat"",
    ""rhymes"": [
      ""note"",
      ""promote"",
      ""float"",
      ""quote"",
      ""goat"",
      ""vote"",
      ""devote""
    ],
    ""soundsLike"": [
      ""bid""
    ]
  },
  {
    ""word"": ""body"",
    ""rhymes"": [
      ""embody""
    ],
    ""soundsLike"": [
      ""buddy""
    ]
  },
  {
    ""word"": ""boil"",
    ""rhymes"": [
      ""oil"",
      ""foil"",
      ""spoil"",
      ""coil""
    ],
    ""soundsLike"": [
      ""ball"",
      ""boy""
    ]
  },
  {
    ""word"": ""bomb"",
    ""rhymes"": [
      ""palm"",
      ""calm"",
      ""mom""
    ],
    ""soundsLike"": [
      ""palm"",
      ""bean""
    ]
  },
  {
    ""word"": ""bone"",
    ""rhymes"": [
      ""zone"",
      ""stone"",
      ""tone"",
      ""loan"",
      ""phone"",
      ""own"",
      ""alone"",
      ""ozone"",
      ""unknown""
    ],
    ""soundsLike"": [
      ""bean""
    ]
  },
  {
    ""word"": ""bonus"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""business"",
      ""bounce""
    ]
  },
  {
    ""word"": ""book"",
    ""rhymes"": [
      ""cook""
    ],
    ""soundsLike"": [
      ""bike"",
      ""bag""
    ]
  },
  {
    ""word"": ""boost"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""best"",
      ""post"",
      ""base""
    ]
  },
  {
    ""word"": ""border"",
    ""rhymes"": [
      ""order"",
      ""disorder""
    ],
    ""soundsLike"": [
      ""board""
    ]
  },
  {
    ""word"": ""boring"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""bring""
    ]
  },
  {
    ""word"": ""borrow"",
    ""rhymes"": [
      ""know"",
      ""snow"",
      ""throw"",
      ""hello"",
      ""grow"",
      ""shadow"",
      ""potato"",
      ""toe"",
      ""glow"",
      ""slow"",
      ""tomato"",
      ""below"",
      ""radio"",
      ""meadow"",
      ""tomorrow"",
      ""photo"",
      ""tornado"",
      ""buffalo""
    ],
    ""soundsLike"": [
      ""bar""
    ]
  },
  {
    ""word"": ""boss"",
    ""rhymes"": [
      ""cross"",
      ""sauce"",
      ""across"",
      ""toss""
    ],
    ""soundsLike"": [
      ""bus"",
      ""base""
    ]
  },
  {
    ""word"": ""bottom"",
    ""rhymes"": [
      ""autumn""
    ],
    ""soundsLike"": []
  },
  {
    ""word"": ""bounce"",
    ""rhymes"": [
      ""announce""
    ],
    ""soundsLike"": [
      ""boss""
    ]
  },
  {
    ""word"": ""box"",
    ""rhymes"": [
      ""fox""
    ],
    ""soundsLike"": [
      ""boss""
    ]
  },
  {
    ""word"": ""boy"",
    ""rhymes"": [
      ""employ"",
      ""joy"",
      ""enjoy"",
      ""destroy"",
      ""toy""
    ],
    ""soundsLike"": [
      ""boil""
    ]
  },
  {
    ""word"": ""bracket"",
    ""rhymes"": [
      ""jacket""
    ],
    ""soundsLike"": []
  },
  {
    ""word"": ""brain"",
    ""rhymes"": [
      ""train"",
      ""domain"",
      ""sustain"",
      ""rain"",
      ""again"",
      ""grain"",
      ""obtain"",
      ""gain"",
      ""explain"",
      ""main"",
      ""crane"",
      ""insane"",
      ""remain""
    ],
    ""soundsLike"": [
      ""brown"",
      ""bring"",
      ""broom""
    ]
  },
  {
    ""word"": ""brand"",
    ""rhymes"": [
      ""hand"",
      ""stand"",
      ""demand"",
      ""expand"",
      ""sand""
    ],
    ""soundsLike"": [
      ""brain"",
      ""beyond"",
      ""bind""
    ]
  },
  {
    ""word"": ""brass"",
    ""rhymes"": [
      ""grass"",
      ""pass"",
      ""glass"",
      ""mass"",
      ""gas""
    ],
    ""soundsLike"": [
      ""breeze""
    ]
  },
  {
    ""word"": ""brave"",
    ""rhymes"": [
      ""wave"",
      ""save"",
      ""cave"",
      ""behave"",
      ""pave""
    ],
    ""soundsLike"": []
  },
  {
    ""word"": ""bread"",
    ""rhymes"": [
      ""head"",
      ""spread"",
      ""shed"",
      ""ahead""
    ],
    ""soundsLike"": [
      ""bright"",
      ""bird"",
      ""proud"",
      ""pride"",
      ""parade"",
      ""bid""
    ]
  },
  {
    ""word"": ""breeze"",
    ""rhymes"": [
      ""disease"",
      ""cheese"",
      ""squeeze"",
      ""please""
    ],
    ""soundsLike"": [
      ""brass""
    ]
  },
  {
    ""word"": ""brick"",
    ""rhymes"": [
      ""stick"",
      ""trick"",
      ""kick"",
      ""quick"",
      ""sick"",
      ""click"",
      ""picnic""
    ],
    ""soundsLike"": [
      ""brisk""
    ]
  },
  {
    ""word"": ""bridge"",
    ""rhymes"": [
      ""ridge""
    ],
    ""soundsLike"": [
      ""brush"",
      ""bring"",
      ""badge""
    ]
  },
  {
    ""word"": ""brief"",
    ""rhymes"": [
      ""leaf"",
      ""relief"",
      ""chief"",
      ""grief"",
      ""beef""
    ],
    ""soundsLike"": [
      ""beef"",
      ""proof"",
      ""brave""
    ]
  },
  {
    ""word"": ""bright"",
    ""rhymes"": [
      ""light"",
      ""right"",
      ""night"",
      ""write"",
      ""sight"",
      ""flight"",
      ""slight"",
      ""height"",
      ""kite"",
      ""invite"",
      ""excite"",
      ""midnight""
    ],
    ""soundsLike"": [
      ""bread"",
      ""burst"",
      ""pride""
    ]
  },
  {
    ""word"": ""bring"",
    ""rhymes"": [
      ""ring"",
      ""spring"",
      ""swing"",
      ""sting"",
      ""thing"",
      ""wing"",
      ""sing""
    ],
    ""soundsLike"": [
      ""brain"",
      ""brown"",
      ""boring"",
      ""bridge"",
      ""broom""
    ]
  },
  {
    ""word"": ""brisk"",
    ""rhymes"": [
      ""risk""
    ],
    ""soundsLike"": [
      ""brick"",
      ""brass""
    ]
  },
  {
    ""word"": ""broccoli"",
    ""rhymes"": [],
    ""soundsLike"": []
  },
  {
    ""word"": ""broken"",
    ""rhymes"": [
      ""token""
    ],
    ""soundsLike"": [
      ""bacon""
    ]
  },
  {
    ""word"": ""bronze"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""brown""
    ]
  },
  {
    ""word"": ""broom"",
    ""rhymes"": [
      ""room"",
      ""assume"",
      ""gloom""
    ],
    ""soundsLike"": [
      ""brain"",
      ""brown"",
      ""bring""
    ]
  },
  {
    ""word"": ""brother"",
    ""rhymes"": [
      ""other"",
      ""rather"",
      ""another"",
      ""mother""
    ],
    ""soundsLike"": []
  },
  {
    ""word"": ""brown"",
    ""rhymes"": [
      ""around"",
      ""town"",
      ""frown"",
      ""gown"",
      ""clown""
    ],
    ""soundsLike"": [
      ""brain"",
      ""bring"",
      ""broom""
    ]
  },
  {
    ""word"": ""brush"",
    ""rhymes"": [
      ""flush"",
      ""crush"",
      ""blush"",
      ""slush""
    ],
    ""soundsLike"": [
      ""bridge""
    ]
  },
  {
    ""word"": ""bubble"",
    ""rhymes"": [
      ""double"",
      ""trouble""
    ],
    ""soundsLike"": [
      ""people""
    ]
  },
  {
    ""word"": ""buddy"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""body""
    ]
  },
  {
    ""word"": ""budget"",
    ""rhymes"": [],
    ""soundsLike"": []
  },
  {
    ""word"": ""buffalo"",
    ""rhymes"": [
      ""know"",
      ""snow"",
      ""throw"",
      ""hello"",
      ""grow"",
      ""shadow"",
      ""potato"",
      ""toe"",
      ""glow"",
      ""slow"",
      ""tomato"",
      ""below"",
      ""radio"",
      ""meadow"",
      ""tomorrow"",
      ""borrow"",
      ""photo"",
      ""tornado""
    ],
    ""soundsLike"": [
      ""below""
    ]
  },
  {
    ""word"": ""build"",
    ""rhymes"": [
      ""rebuild""
    ],
    ""soundsLike"": [
      ""belt"",
      ""bid""
    ]
  },
  {
    ""word"": ""bulb"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""pulp"",
      ""ball"",
      ""boil""
    ]
  },
  {
    ""word"": ""bulk"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""below""
    ]
  },
  {
    ""word"": ""bullet"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""pilot"",
      ""belt""
    ]
  },
  {
    ""word"": ""bundle"",
    ""rhymes"": [],
    ""soundsLike"": []
  },
  {
    ""word"": ""bunker"",
    ""rhymes"": [],
    ""soundsLike"": []
  },
  {
    ""word"": ""burden"",
    ""rhymes"": [],
    ""soundsLike"": []
  },
  {
    ""word"": ""burger"",
    ""rhymes"": [],
    ""soundsLike"": []
  },
  {
    ""word"": ""burst"",
    ""rhymes"": [
      ""first""
    ],
    ""soundsLike"": [
      ""best"",
      ""bright"",
      ""bird""
    ]
  },
  {
    ""word"": ""bus"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""boss"",
      ""base"",
      ""buzz""
    ]
  },
  {
    ""word"": ""business"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""bonus""
    ]
  },
  {
    ""word"": ""busy"",
    ""rhymes"": [
      ""dizzy""
    ],
    ""soundsLike"": []
  },
  {
    ""word"": ""butter"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""better"",
      ""bitter""
    ]
  },
  {
    ""word"": ""buyer"",
    ""rhymes"": [
      ""fire"",
      ""wire"",
      ""acquire"",
      ""require"",
      ""hire"",
      ""entire"",
      ""liar"",
      ""retire""
    ],
    ""soundsLike"": [
      ""bar"",
      ""blur""
    ]
  },
  {
    ""word"": ""buzz"",
    ""rhymes"": [
      ""because""
    ],
    ""soundsLike"": [
      ""bus"",
      ""boss"",
      ""base""
    ]
  },
  {
    ""word"": ""cabbage"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""cabin"",
      ""cable""
    ]
  },
  {
    ""word"": ""cabin"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""captain"",
      ""cabbage"",
      ""carbon""
    ]
  },
  {
    ""word"": ""cable"",
    ""rhymes"": [
      ""table"",
      ""label"",
      ""enable"",
      ""able"",
      ""stable"",
      ""unable""
    ],
    ""soundsLike"": [
      ""couple""
    ]
  },
  {
    ""word"": ""cactus"",
    ""rhymes"": [
      ""practice""
    ],
    ""soundsLike"": []
  },
  {
    ""word"": ""cage"",
    ""rhymes"": [
      ""gauge"",
      ""age"",
      ""engage"",
      ""stage"",
      ""page"",
      ""wage""
    ],
    ""soundsLike"": [
      ""gauge"",
      ""cash"",
      ""couch"",
      ""catch"",
      ""coach""
    ]
  },
  {
    ""word"": ""cake"",
    ""rhymes"": [
      ""awake"",
      ""make"",
      ""snake"",
      ""mistake"",
      ""lake"",
      ""steak""
    ],
    ""soundsLike"": [
      ""cook"",
      ""kick""
    ]
  },
  {
    ""word"": ""call"",
    ""rhymes"": [
      ""all"",
      ""ball"",
      ""fall"",
      ""wall"",
      ""recall"",
      ""small"",
      ""alcohol"",
      ""doll"",
      ""install"",
      ""crawl""
    ],
    ""soundsLike"": [
      ""cool"",
      ""coil""
    ]
  },
  {
    ""word"": ""calm"",
    ""rhymes"": [
      ""bomb"",
      ""palm"",
      ""mom""
    ],
    ""soundsLike"": [
      ""come""
    ]
  },
  {
    ""word"": ""camera"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""carry""
    ]
  },
  {
    ""word"": ""camp"",
    ""rhymes"": [
      ""lamp"",
      ""stamp"",
      ""ramp"",
      ""damp""
    ],
    ""soundsLike"": [
      ""can"",
      ""calm""
    ]
  },
  {
    ""word"": ""can"",
    ""rhymes"": [
      ""man"",
      ""fan"",
      ""scan"",
      ""van""
    ],
    ""soundsLike"": [
      ""coin"",
      ""keen""
    ]
  },
  {
    ""word"": ""canal"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""canoe""
    ]
  },
  {
    ""word"": ""cancel"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""castle""
    ]
  },
  {
    ""word"": ""candy"",
    ""rhymes"": [],
    ""soundsLike"": []
  },
  {
    ""word"": ""cannon"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""common"",
      ""canyon""
    ]
  },
  {
    ""word"": ""canoe"",
    ""rhymes"": [
      ""blue"",
      ""into"",
      ""you"",
      ""view"",
      ""review"",
      ""true"",
      ""two"",
      ""shoe"",
      ""crew"",
      ""tissue"",
      ""few"",
      ""glue"",
      ""bamboo"",
      ""undo"",
      ""tattoo"",
      ""renew"",
      ""zoo""
    ],
    ""soundsLike"": [
      ""can""
    ]
  },
  {
    ""word"": ""canvas"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""convince""
    ]
  },
  {
    ""word"": ""canyon"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""cannon""
    ]
  },
  {
    ""word"": ""capable"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""cable"",
      ""couple"",
      ""capital""
    ]
  },
  {
    ""word"": ""capital"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""cattle"",
      ""captain"",
      ""capable"",
      ""casual""
    ]
  },
  {
    ""word"": ""captain"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""kitten"",
      ""cotton"",
      ""cabin""
    ]
  },
  {
    ""word"": ""car"",
    ""rhymes"": [
      ""bar"",
      ""jar"",
      ""radar"",
      ""guitar"",
      ""seminar"",
      ""jaguar"",
      ""cigar""
    ],
    ""soundsLike"": [
      ""core"",
      ""card""
    ]
  },
  {
    ""word"": ""carbon"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""cabin""
    ]
  },
  {
    ""word"": ""card"",
    ""rhymes"": [
      ""guard"",
      ""hard"",
      ""yard""
    ],
    ""soundsLike"": [
      ""cart"",
      ""guard"",
      ""car"",
      ""hard"",
      ""yard""
    ]
  },
  {
    ""word"": ""cargo"",
    ""rhymes"": [
      ""know"",
      ""snow"",
      ""throw"",
      ""hello"",
      ""grow"",
      ""shadow"",
      ""potato"",
      ""toe"",
      ""glow"",
      ""slow"",
      ""tomato"",
      ""below"",
      ""radio"",
      ""meadow"",
      ""tomorrow"",
      ""borrow"",
      ""photo"",
      ""tornado"",
      ""buffalo""
    ],
    ""soundsLike"": []
  },
  {
    ""word"": ""carpet"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""carbon""
    ]
  },
  {
    ""word"": ""carry"",
    ""rhymes"": [
      ""cherry"",
      ""library"",
      ""primary"",
      ""very"",
      ""ordinary"",
      ""merry"",
      ""february""
    ],
    ""soundsLike"": []
  },
  {
    ""word"": ""cart"",
    ""rhymes"": [
      ""art"",
      ""smart"",
      ""heart"",
      ""start"",
      ""apart"",
      ""depart""
    ],
    ""soundsLike"": [
      ""card"",
      ""car"",
      ""caught""
    ]
  },
  {
    ""word"": ""case"",
    ""rhymes"": [
      ""grace"",
      ""face"",
      ""base"",
      ""place"",
      ""space"",
      ""race"",
      ""embrace"",
      ""chase"",
      ""replace"",
      ""erase""
    ],
    ""soundsLike"": [
      ""kiss"",
      ""cause"",
      ""chaos""
    ]
  },
  {
    ""word"": ""cash"",
    ""rhymes"": [
      ""dash"",
      ""flash"",
      ""crash"",
      ""trash""
    ],
    ""soundsLike"": [
      ""catch"",
      ""cat"",
      ""couch"",
      ""cage"",
      ""crash"",
      ""coach""
    ]
  },
  {
    ""word"": ""casino"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""cousin""
    ]
  },
  {
    ""word"": ""castle"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""cancel"",
      ""cattle""
    ]
  },
  {
    ""word"": ""casual"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""usual"",
      ""visual"",
      ""capital"",
      ""actual"",
      ""cancel""
    ]
  },
  {
    ""word"": ""cat"",
    ""rhymes"": [
      ""that"",
      ""hat"",
      ""flat"",
      ""fat"",
      ""chat""
    ],
    ""soundsLike"": [
      ""caught"",
      ""kite"",
      ""kit"",
      ""cash""
    ]
  },
  {
    ""word"": ""catalog"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""cattle""
    ]
  },
  {
    ""word"": ""catch"",
    ""rhymes"": [
      ""match"",
      ""patch""
    ],
    ""soundsLike"": [
      ""cash"",
      ""couch"",
      ""coach"",
      ""can"",
      ""cat"",
      ""cage""
    ]
  },
  {
    ""word"": ""category"",
    ""rhymes"": [
      ""story"",
      ""glory""
    ],
    ""soundsLike"": []
  },
  {
    ""word"": ""cattle"",
    ""rhymes"": [
      ""battle""
    ],
    ""soundsLike"": [
      ""castle""
    ]
  },
  {
    ""word"": ""caught"",
    ""rhymes"": [
      ""spot"",
      ""thought"",
      ""slot"",
      ""robot""
    ],
    ""soundsLike"": [
      ""cat"",
      ""kite"",
      ""kit"",
      ""cause"",
      ""cart""
    ]
  },
  {
    ""word"": ""cause"",
    ""rhymes"": [
      ""because"",
      ""pause""
    ],
    ""soundsLike"": [
      ""car"",
      ""case""
    ]
  },
  {
    ""word"": ""caution"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""cushion"",
      ""kitchen"",
      ""cotton""
    ]
  },
  {
    ""word"": ""cave"",
    ""rhymes"": [
      ""wave"",
      ""brave"",
      ""save"",
      ""behave"",
      ""pave""
    ],
    ""soundsLike"": [
      ""give"",
      ""cage""
    ]
  },
  {
    ""word"": ""ceiling"",
    ""rhymes"": [],
    ""soundsLike"": []
  },
  {
    ""word"": ""celery"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""silly""
    ]
  },
  {
    ""word"": ""cement"",
    ""rhymes"": [
      ""present"",
      ""rent"",
      ""prevent"",
      ""segment"",
      ""tent"",
      ""orient"",
      ""frequent""
    ],
    ""soundsLike"": [
      ""segment"",
      ""salmon""
    ]
  },
  {
    ""word"": ""census"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""sense""
    ]
  },
  {
    ""word"": ""century"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""celery""
    ]
  },
  {
    ""word"": ""cereal"",
    ""rhymes"": [
      ""material""
    ],
    ""soundsLike"": [
      ""civil"",
      ""circle"",
      ""series""
    ]
  },
  {
    ""word"": ""certain"",
    ""rhymes"": [
      ""curtain""
    ],
    ""soundsLike"": [
      ""stone"",
      ""sudden"",
      ""curtain""
    ]
  },
  {
    ""word"": ""chair"",
    ""rhymes"": [
      ""air"",
      ""wear"",
      ""square"",
      ""share"",
      ""despair"",
      ""there"",
      ""hair"",
      ""spare"",
      ""aware"",
      ""pair"",
      ""where"",
      ""rare"",
      ""swear"",
      ""pear"",
      ""affair"",
      ""repair"",
      ""prepare"",
      ""glare"",
      ""scare"",
      ""warfare"",
      ""unfair"",
      ""unaware""
    ],
    ""soundsLike"": [
      ""share"",
      ""cherry"",
      ""hair"",
      ""jar""
    ]
  },
  {
    ""word"": ""chalk"",
    ""rhymes"": [
      ""stock"",
      ""talk"",
      ""lock"",
      ""clock"",
      ""walk"",
      ""shock"",
      ""hawk"",
      ""flock"",
      ""knock"",
      ""sock"",
      ""unlock""
    ],
    ""soundsLike"": [
      ""check"",
      ""shock""
    ]
  },
  {
    ""word"": ""champion"",
    ""rhymes"": [],
    ""soundsLike"": []
  },
  {
    ""word"": ""change"",
    ""rhymes"": [
      ""range"",
      ""exchange"",
      ""arrange""
    ],
    ""soundsLike"": [
      ""range"",
      ""lounge"",
      ""arrange""
    ]
  },
  {
    ""word"": ""chaos"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""case"",
      ""cause""
    ]
  },
  {
    ""word"": ""chapter"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""chat""
    ]
  },
  {
    ""word"": ""charge"",
    ""rhymes"": [
      ""large""
    ],
    ""soundsLike"": [
      ""large"",
      ""chair"",
      ""cherry"",
      ""harsh"",
      ""jar""
    ]
  },
  {
    ""word"": ""chase"",
    ""rhymes"": [
      ""case"",
      ""grace"",
      ""face"",
      ""base"",
      ""place"",
      ""space"",
      ""race"",
      ""embrace"",
      ""replace"",
      ""erase""
    ],
    ""soundsLike"": [
      ""choice"",
      ""cheese"",
      ""choose"",
      ""case"",
      ""juice""
    ]
  },
  {
    ""word"": ""chat"",
    ""rhymes"": [
      ""cat"",
      ""that"",
      ""hat"",
      ""flat"",
      ""fat""
    ],
    ""soundsLike"": [
      ""cat"",
      ""hat""
    ]
  },
  {
    ""word"": ""cheap"",
    ""rhymes"": [
      ""keep"",
      ""sleep""
    ],
    ""soundsLike"": [
      ""keep""
    ]
  },
  {
    ""word"": ""check"",
    ""rhymes"": [
      ""neck"",
      ""wreck""
    ],
    ""soundsLike"": [
      ""chalk""
    ]
  },
  {
    ""word"": ""cheese"",
    ""rhymes"": [
      ""disease"",
      ""squeeze"",
      ""breeze"",
      ""please""
    ],
    ""soundsLike"": [
      ""choose"",
      ""choice"",
      ""chase""
    ]
  },
  {
    ""word"": ""chef"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""chief"",
      ""shift"",
      ""shove""
    ]
  },
  {
    ""word"": ""cherry"",
    ""rhymes"": [
      ""carry"",
      ""library"",
      ""primary"",
      ""very"",
      ""ordinary"",
      ""merry"",
      ""february""
    ],
    ""soundsLike"": [
      ""chair"",
      ""carry""
    ]
  },
  {
    ""word"": ""chest"",
    ""rhymes"": [
      ""test"",
      ""best"",
      ""suggest"",
      ""nest"",
      ""west"",
      ""arrest"",
      ""invest""
    ],
    ""soundsLike"": [
      ""just"",
      ""test"",
      ""west"",
      ""nest"",
      ""choice"",
      ""chase"",
      ""chat""
    ]
  },
  {
    ""word"": ""chicken"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""second""
    ]
  },
  {
    ""word"": ""chief"",
    ""rhymes"": [
      ""leaf"",
      ""relief"",
      ""brief"",
      ""grief"",
      ""beef""
    ],
    ""soundsLike"": [
      ""leaf"",
      ""chef""
    ]
  },
  {
    ""word"": ""child"",
    ""rhymes"": [
      ""wild""
    ],
    ""soundsLike"": [
      ""wild"",
      ""shield""
    ]
  },
  {
    ""word"": ""chimney"",
    ""rhymes"": [],
    ""soundsLike"": []
  },
  {
    ""word"": ""choice"",
    ""rhymes"": [
      ""voice""
    ],
    ""soundsLike"": [
      ""chase"",
      ""cheese"",
      ""choose"",
      ""voice"",
      ""juice""
    ]
  },
  {
    ""word"": ""choose"",
    ""rhymes"": [
      ""abuse"",
      ""news"",
      ""refuse"",
      ""cruise"",
      ""excuse"",
      ""accuse""
    ],
    ""soundsLike"": [
      ""cheese"",
      ""choice"",
      ""chase"",
      ""jazz""
    ]
  },
  {
    ""word"": ""chronic"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""comic"",
      ""clinic""
    ]
  },
  {
    ""word"": ""chuckle"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""cycle""
    ]
  },
  {
    ""word"": ""chunk"",
    ""rhymes"": [
      ""junk""
    ],
    ""soundsLike"": [
      ""junk"",
      ""check"",
      ""chalk""
    ]
  },
  {
    ""word"": ""churn"",
    ""rhymes"": [
      ""turn"",
      ""learn"",
      ""return"",
      ""earn""
    ],
    ""soundsLike"": [
      ""learn"",
      ""turn""
    ]
  },
  {
    ""word"": ""cigar"",
    ""rhymes"": [
      ""bar"",
      ""car"",
      ""jar"",
      ""radar"",
      ""guitar"",
      ""seminar"",
      ""jaguar""
    ],
    ""soundsLike"": []
  },
  {
    ""word"": ""cinnamon"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""minimum""
    ]
  },
  {
    ""word"": ""circle"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""cycle"",
      ""skull"",
      ""skill"",
      ""school"",
      ""scale""
    ]
  },
  {
    ""word"": ""citizen"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""season""
    ]
  },
  {
    ""word"": ""city"",
    ""rhymes"": [
      ""pretty""
    ],
    ""soundsLike"": []
  },
  {
    ""word"": ""civil"",
    ""rhymes"": [],
    ""soundsLike"": []
  },
  {
    ""word"": ""claim"",
    ""rhymes"": [
      ""game"",
      ""name"",
      ""frame"",
      ""aim"",
      ""same"",
      ""blame"",
      ""flame"",
      ""fame""
    ],
    ""soundsLike"": [
      ""climb"",
      ""clean"",
      ""clown"",
      ""clay"",
      ""gloom"",
      ""calm""
    ]
  },
  {
    ""word"": ""clap"",
    ""rhymes"": [
      ""snap"",
      ""trap"",
      ""gap"",
      ""wrap"",
      ""scrap""
    ],
    ""soundsLike"": [
      ""clip"",
      ""club"",
      ""clay"",
      ""claw"",
      ""clump""
    ]
  },
  {
    ""word"": ""clarify"",
    ""rhymes"": [
      ""fly"",
      ""eye"",
      ""high"",
      ""supply"",
      ""identify"",
      ""dry"",
      ""shy"",
      ""cry"",
      ""try"",
      ""verify"",
      ""deny"",
      ""defy"",
      ""rely"",
      ""modify"",
      ""satisfy"",
      ""spy""
    ],
    ""soundsLike"": [
      ""verify""
    ]
  },
  {
    ""word"": ""claw"",
    ""rhymes"": [
      ""law"",
      ""draw"",
      ""raw""
    ],
    ""soundsLike"": [
      ""clay"",
      ""cloth"",
      ""glow"",
      ""glue"",
      ""clock"",
      ""clog""
    ]
  },
  {
    ""word"": ""clay"",
    ""rhymes"": [
      ""day"",
      ""away"",
      ""way"",
      ""play"",
      ""say"",
      ""survey"",
      ""display"",
      ""stay"",
      ""delay"",
      ""holiday"",
      ""essay"",
      ""betray"",
      ""spray"",
      ""they"",
      ""okay"",
      ""tray"",
      ""obey"",
      ""today""
    ],
    ""soundsLike"": [
      ""claw"",
      ""claim"",
      ""glow"",
      ""glue""
    ]
  },
  {
    ""word"": ""clean"",
    ""rhymes"": [
      ""mean"",
      ""green"",
      ""bean"",
      ""machine"",
      ""keen"",
      ""screen"",
      ""between"",
      ""scene"",
      ""marine""
    ],
    ""soundsLike"": [
      ""clown"",
      ""climb"",
      ""claim"",
      ""keen""
    ]
  },
  {
    ""word"": ""clerk"",
    ""rhymes"": [
      ""work"",
      ""network"",
      ""artwork""
    ],
    ""soundsLike"": [
      ""clock"",
      ""click"",
      ""clog"",
      ""cook""
    ]
  },
  {
    ""word"": ""clever"",
    ""rhymes"": [
      ""never""
    ],
    ""soundsLike"": [
      ""cover"",
      ""glare""
    ]
  },
  {
    ""word"": ""click"",
    ""rhymes"": [
      ""stick"",
      ""trick"",
      ""kick"",
      ""quick"",
      ""sick"",
      ""brick"",
      ""picnic""
    ],
    ""soundsLike"": [
      ""clock"",
      ""quick"",
      ""kick"",
      ""clerk"",
      ""clog""
    ]
  },
  {
    ""word"": ""client"",
    ""rhymes"": [
      ""giant""
    ],
    ""soundsLike"": [
      ""current""
    ]
  },
  {
    ""word"": ""cliff"",
    ""rhymes"": [
      ""sniff""
    ],
    ""soundsLike"": [
      ""click"",
      ""clip"",
      ""cloth"",
      ""clay"",
      ""claw""
    ]
  },
  {
    ""word"": ""climb"",
    ""rhymes"": [
      ""time"",
      ""crime""
    ],
    ""soundsLike"": [
      ""claim"",
      ""clean"",
      ""clown"",
      ""gloom"",
      ""calm""
    ]
  },
  {
    ""word"": ""clinic"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""click"",
      ""chronic"",
      ""comic""
    ]
  },
  {
    ""word"": ""clip"",
    ""rhymes"": [
      ""ship"",
      ""trip"",
      ""tip"",
      ""flip"",
      ""whip"",
      ""drip"",
      ""hip"",
      ""equip""
    ],
    ""soundsLike"": [
      ""clap"",
      ""club"",
      ""cup"",
      ""clump"",
      ""clay"",
      ""claw""
    ]
  },
  {
    ""word"": ""clock"",
    ""rhymes"": [
      ""stock"",
      ""talk"",
      ""lock"",
      ""walk"",
      ""shock"",
      ""hawk"",
      ""flock"",
      ""knock"",
      ""chalk"",
      ""sock"",
      ""unlock""
    ],
    ""soundsLike"": [
      ""click"",
      ""clog"",
      ""clerk"",
      ""cloth"",
      ""claw""
    ]
  },
  {
    ""word"": ""clog"",
    ""rhymes"": [
      ""dog"",
      ""frog"",
      ""fog"",
      ""hedgehog""
    ],
    ""soundsLike"": [
      ""clock"",
      ""click"",
      ""claw"",
      ""clay"",
      ""clerk""
    ]
  },
  {
    ""word"": ""close"",
    ""rhymes"": [
      ""rose"",
      ""nose"",
      ""impose"",
      ""expose"",
      ""dose"",
      ""oppose""
    ],
    ""soundsLike"": [
      ""glass"",
      ""case""
    ]
  },
  {
    ""word"": ""cloth"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""claw"",
      ""clock"",
      ""climb"",
      ""click"",
      ""cliff"",
      ""claim"",
      ""clay""
    ]
  },
  {
    ""word"": ""cloud"",
    ""rhymes"": [
      ""proud"",
      ""crowd"",
      ""loud""
    ],
    ""soundsLike"": [
      ""glad"",
      ""glide"",
      ""claw""
    ]
  },
  {
    ""word"": ""clown"",
    ""rhymes"": [
      ""around"",
      ""brown"",
      ""town"",
      ""frown"",
      ""gown""
    ],
    ""soundsLike"": [
      ""clean"",
      ""climb"",
      ""claim"",
      ""claw""
    ]
  },
  {
    ""word"": ""club"",
    ""rhymes"": [
      ""hub"",
      ""scrub""
    ],
    ""soundsLike"": [
      ""clip"",
      ""clap"",
      ""globe"",
      ""cup"",
      ""cube"",
      ""clay"",
      ""claw"",
      ""clump""
    ]
  },
  {
    ""word"": ""clump"",
    ""rhymes"": [
      ""jump""
    ],
    ""soundsLike"": [
      ""clip"",
      ""club"",
      ""camp"",
      ""climb"",
      ""claim"",
      ""clap""
    ]
  },
  {
    ""word"": ""cluster"",
    ""rhymes"": [],
    ""soundsLike"": []
  },
  {
    ""word"": ""clutch"",
    ""rhymes"": [
      ""such"",
      ""much"",
      ""dutch""
    ],
    ""soundsLike"": [
      ""click"",
      ""catch"",
      ""claw""
    ]
  },
  {
    ""word"": ""coach"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""couch"",
      ""catch"",
      ""cash"",
      ""cage""
    ]
  },
  {
    ""word"": ""coast"",
    ""rhymes"": [
      ""post"",
      ""host"",
      ""roast"",
      ""ghost"",
      ""toast"",
      ""almost""
    ],
    ""soundsLike"": [
      ""cost"",
      ""ghost"",
      ""host"",
      ""case""
    ]
  },
  {
    ""word"": ""coconut"",
    ""rhymes"": [
      ""that"",
      ""nut"",
      ""what"",
      ""robot"",
      ""walnut"",
      ""peanut""
    ],
    ""soundsLike"": []
  },
  {
    ""word"": ""code"",
    ""rhymes"": [
      ""road"",
      ""load"",
      ""episode"",
      ""erode""
    ],
    ""soundsLike"": [
      ""kid"",
      ""cat"",
      ""caught""
    ]
  },
  {
    ""word"": ""coffee"",
    ""rhymes"": [],
    ""soundsLike"": []
  },
  {
    ""word"": ""coil"",
    ""rhymes"": [
      ""oil"",
      ""foil"",
      ""spoil"",
      ""boil""
    ],
    ""soundsLike"": [
      ""cool"",
      ""call""
    ]
  },
  {
    ""word"": ""coin"",
    ""rhymes"": [
      ""join""
    ],
    ""soundsLike"": [
      ""can"",
      ""keen"",
      ""calm""
    ]
  },
  {
    ""word"": ""collect"",
    ""rhymes"": [
      ""project"",
      ""subject"",
      ""aspect"",
      ""object"",
      ""perfect"",
      ""reflect"",
      ""direct"",
      ""connect"",
      ""expect"",
      ""correct"",
      ""neglect"",
      ""suspect"",
      ""select"",
      ""protect"",
      ""reject"",
      ""detect"",
      ""insect"",
      ""inject""
    ],
    ""soundsLike"": [
      ""connect"",
      ""select"",
      ""correct""
    ]
  },
  {
    ""word"": ""color"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""culture""
    ]
  },
  {
    ""word"": ""column"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""calm""
    ]
  },
  {
    ""word"": ""combine"",
    ""rhymes"": [
      ""design"",
      ""sign"",
      ""fine"",
      ""wine"",
      ""genuine"",
      ""shine"",
      ""decline"",
      ""define"",
      ""online""
    ],
    ""soundsLike"": [
      ""common"",
      ""cabin""
    ]
  },
  {
    ""word"": ""come"",
    ""rhymes"": [
      ""become"",
      ""income"",
      ""thumb"",
      ""drum"",
      ""dumb""
    ],
    ""soundsLike"": [
      ""can"",
      ""calm""
    ]
  },
  {
    ""word"": ""comfort"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""concert"",
      ""confirm"",
      ""camera""
    ]
  },
  {
    ""word"": ""comic"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""chronic""
    ]
  },
  {
    ""word"": ""common"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""cannon""
    ]
  },
  {
    ""word"": ""company"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""combine""
    ]
  },
  {
    ""word"": ""concert"",
    ""rhymes"": [
      ""desert"",
      ""hurt"",
      ""alert"",
      ""skirt"",
      ""divert"",
      ""dirt""
    ],
    ""soundsLike"": [
      ""cost"",
      ""cart""
    ]
  },
  {
    ""word"": ""conduct"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""connect""
    ]
  },
  {
    ""word"": ""confirm"",
    ""rhymes"": [
      ""term"",
      ""firm""
    ],
    ""soundsLike"": [
      ""comfort""
    ]
  },
  {
    ""word"": ""congress"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""kangaroo""
    ]
  },
  {
    ""word"": ""connect"",
    ""rhymes"": [
      ""project"",
      ""subject"",
      ""aspect"",
      ""object"",
      ""perfect"",
      ""reflect"",
      ""direct"",
      ""expect"",
      ""correct"",
      ""neglect"",
      ""suspect"",
      ""collect"",
      ""select"",
      ""protect"",
      ""reject"",
      ""detect"",
      ""insect"",
      ""inject""
    ],
    ""soundsLike"": [
      ""conduct"",
      ""collect"",
      ""correct""
    ]
  },
  {
    ""word"": ""consider"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""concert""
    ]
  },
  {
    ""word"": ""control"",
    ""rhymes"": [
      ""hole"",
      ""pole"",
      ""soul"",
      ""enroll"",
      ""patrol""
    ],
    ""soundsLike"": [
      ""country"",
      ""canal""
    ]
  },
  {
    ""word"": ""convince"",
    ""rhymes"": [
      ""since""
    ],
    ""soundsLike"": [
      ""canvas""
    ]
  },
  {
    ""word"": ""cook"",
    ""rhymes"": [
      ""book""
    ],
    ""soundsLike"": [
      ""cake"",
      ""kick""
    ]
  },
  {
    ""word"": ""cool"",
    ""rhymes"": [
      ""school"",
      ""rule"",
      ""pool"",
      ""tool"",
      ""fuel"",
      ""stool"",
      ""cruel"",
      ""mule"",
      ""jewel""
    ],
    ""soundsLike"": [
      ""call"",
      ""coil""
    ]
  },
  {
    ""word"": ""copper"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""car"",
      ""copy"",
      ""core""
    ]
  },
  {
    ""word"": ""copy"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""copper""
    ]
  },
  {
    ""word"": ""coral"",
    ""rhymes"": [
      ""moral""
    ],
    ""soundsLike"": []
  },
  {
    ""word"": ""core"",
    ""rhymes"": [
      ""door"",
      ""more"",
      ""floor"",
      ""before"",
      ""ignore"",
      ""dinosaur"",
      ""outdoor"",
      ""indoor""
    ],
    ""soundsLike"": [
      ""car"",
      ""cause""
    ]
  },
  {
    ""word"": ""corn"",
    ""rhymes"": [
      ""horn""
    ],
    ""soundsLike"": [
      ""core""
    ]
  },
  {
    ""word"": ""correct"",
    ""rhymes"": [
      ""project"",
      ""subject"",
      ""aspect"",
      ""object"",
      ""perfect"",
      ""reflect"",
      ""direct"",
      ""connect"",
      ""expect"",
      ""neglect"",
      ""suspect"",
      ""collect"",
      ""select"",
      ""protect"",
      ""reject"",
      ""detect"",
      ""insect"",
      ""inject""
    ],
    ""soundsLike"": [
      ""cricket"",
      ""crack"",
      ""creek""
    ]
  },
  {
    ""word"": ""cost"",
    ""rhymes"": [
      ""frost"",
      ""exhaust""
    ],
    ""soundsLike"": [
      ""coast"",
      ""caught"",
      ""ghost""
    ]
  },
  {
    ""word"": ""cotton"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""kitten"",
      ""caution"",
      ""curtain""
    ]
  },
  {
    ""word"": ""couch"",
    ""rhymes"": [
      ""crouch""
    ],
    ""soundsLike"": [
      ""catch"",
      ""coach"",
      ""cash"",
      ""cage"",
      ""crouch""
    ]
  },
  {
    ""word"": ""country"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""control""
    ]
  },
  {
    ""word"": ""couple"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""cable""
    ]
  },
  {
    ""word"": ""course"",
    ""rhymes"": [
      ""force"",
      ""horse"",
      ""source"",
      ""endorse"",
      ""enforce"",
      ""divorce""
    ],
    ""soundsLike"": [
      ""core""
    ]
  },
  {
    ""word"": ""cousin"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""kitchen"",
      ""kitten"",
      ""caution""
    ]
  },
  {
    ""word"": ""cover"",
    ""rhymes"": [
      ""hover"",
      ""discover"",
      ""uncover""
    ],
    ""soundsLike"": [
      ""hover"",
      ""clever"",
      ""car"",
      ""cave""
    ]
  },
  {
    ""word"": ""coyote"",
    ""rhymes"": [],
    ""soundsLike"": []
  },
  {
    ""word"": ""crack"",
    ""rhymes"": [
      ""black"",
      ""track"",
      ""attack"",
      ""rack"",
      ""snack""
    ],
    ""soundsLike"": [
      ""creek""
    ]
  },
  {
    ""word"": ""cradle"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""cruel"",
      ""credit"",
      ""cattle""
    ]
  },
  {
    ""word"": ""craft"",
    ""rhymes"": [
      ""draft"",
      ""shaft""
    ],
    ""soundsLike"": []
  },
  {
    ""word"": ""cram"",
    ""rhymes"": [
      ""program"",
      ""slam"",
      ""diagram""
    ],
    ""soundsLike"": [
      ""crime"",
      ""cream"",
      ""crane""
    ]
  },
  {
    ""word"": ""crane"",
    ""rhymes"": [
      ""train"",
      ""domain"",
      ""sustain"",
      ""rain"",
      ""again"",
      ""grain"",
      ""obtain"",
      ""brain"",
      ""gain"",
      ""explain"",
      ""main"",
      ""insane"",
      ""remain""
    ],
    ""soundsLike"": [
      ""grain"",
      ""crime"",
      ""cream"",
      ""cram""
    ]
  },
  {
    ""word"": ""crash"",
    ""rhymes"": [
      ""dash"",
      ""flash"",
      ""trash"",
      ""cash""
    ],
    ""soundsLike"": [
      ""crush"",
      ""cash"",
      ""crouch"",
      ""crack"",
      ""catch"",
      ""cram""
    ]
  },
  {
    ""word"": ""crater"",
    ""rhymes"": [
      ""later"",
      ""elevator""
    ],
    ""soundsLike"": []
  },
  {
    ""word"": ""crawl"",
    ""rhymes"": [
      ""all"",
      ""ball"",
      ""call"",
      ""fall"",
      ""wall"",
      ""recall"",
      ""small"",
      ""alcohol"",
      ""doll"",
      ""install""
    ],
    ""soundsLike"": [
      ""cruel"",
      ""call"",
      ""crouch""
    ]
  },
  {
    ""word"": ""crazy"",
    ""rhymes"": [
      ""lazy""
    ],
    ""soundsLike"": [
      ""cruise""
    ]
  },
  {
    ""word"": ""cream"",
    ""rhymes"": [
      ""dream"",
      ""scheme"",
      ""team"",
      ""theme"",
      ""supreme""
    ],
    ""soundsLike"": [
      ""crime"",
      ""cram"",
      ""crane""
    ]
  },
  {
    ""word"": ""credit"",
    ""rhymes"": [
      ""edit""
    ],
    ""soundsLike"": [
      ""correct"",
      ""cradle""
    ]
  },
  {
    ""word"": ""creek"",
    ""rhymes"": [
      ""seek"",
      ""unique"",
      ""bleak"",
      ""speak"",
      ""antique""
    ],
    ""soundsLike"": [
      ""crack""
    ]
  },
  {
    ""word"": ""crew"",
    ""rhymes"": [
      ""blue"",
      ""into"",
      ""you"",
      ""view"",
      ""review"",
      ""true"",
      ""two"",
      ""shoe"",
      ""tissue"",
      ""few"",
      ""glue"",
      ""bamboo"",
      ""undo"",
      ""tattoo"",
      ""renew"",
      ""canoe"",
      ""zoo""
    ],
    ""soundsLike"": [
      ""cry"",
      ""cruel"",
      ""cruise""
    ]
  },
  {
    ""word"": ""cricket"",
    ""rhymes"": [
      ""ticket""
    ],
    ""soundsLike"": [
      ""correct""
    ]
  },
  {
    ""word"": ""crime"",
    ""rhymes"": [
      ""time"",
      ""climb""
    ],
    ""soundsLike"": [
      ""cream"",
      ""cram"",
      ""cry"",
      ""crane""
    ]
  },
  {
    ""word"": ""crisp"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""cross"",
      ""crop""
    ]
  },
  {
    ""word"": ""critic"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""chronic"",
      ""arctic""
    ]
  },
  {
    ""word"": ""crop"",
    ""rhymes"": [
      ""top"",
      ""shop"",
      ""drop"",
      ""swap"",
      ""laptop""
    ],
    ""soundsLike"": [
      ""group"",
      ""grape""
    ]
  },
  {
    ""word"": ""cross"",
    ""rhymes"": [
      ""sauce"",
      ""across"",
      ""boss"",
      ""toss""
    ],
    ""soundsLike"": [
      ""cruise"",
      ""across"",
      ""grace"",
      ""grass""
    ]
  },
  {
    ""word"": ""crouch"",
    ""rhymes"": [
      ""couch""
    ],
    ""soundsLike"": [
      ""couch"",
      ""crash"",
      ""crush"",
      ""cross"",
      ""crawl"",
      ""catch""
    ]
  },
  {
    ""word"": ""crowd"",
    ""rhymes"": [
      ""cloud"",
      ""proud"",
      ""loud""
    ],
    ""soundsLike"": [
      ""grid""
    ]
  },
  {
    ""word"": ""crucial"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""cruel"",
      ""cradle""
    ]
  },
  {
    ""word"": ""cruel"",
    ""rhymes"": [
      ""school"",
      ""rule"",
      ""pool"",
      ""cool"",
      ""tool"",
      ""fuel"",
      ""stool"",
      ""mule"",
      ""unusual"",
      ""jewel""
    ],
    ""soundsLike"": [
      ""crucial"",
      ""crawl"",
      ""cradle""
    ]
  },
  {
    ""word"": ""cruise"",
    ""rhymes"": [
      ""choose"",
      ""abuse"",
      ""news"",
      ""refuse"",
      ""excuse"",
      ""accuse""
    ],
    ""soundsLike"": [
      ""crew"",
      ""cross""
    ]
  },
  {
    ""word"": ""crumble"",
    ""rhymes"": [
      ""humble"",
      ""tumble"",
      ""stumble""
    ],
    ""soundsLike"": []
  },
  {
    ""word"": ""crunch"",
    ""rhymes"": [
      ""punch"",
      ""lunch""
    ],
    ""soundsLike"": [
      ""current"",
      ""crush"",
      ""crane"",
      ""crouch""
    ]
  },
  {
    ""word"": ""crush"",
    ""rhymes"": [
      ""flush"",
      ""brush"",
      ""blush"",
      ""slush""
    ],
    ""soundsLike"": [
      ""crash"",
      ""crouch""
    ]
  },
  {
    ""word"": ""cry"",
    ""rhymes"": [
      ""fly"",
      ""eye"",
      ""high"",
      ""supply"",
      ""identify"",
      ""dry"",
      ""shy"",
      ""clarify"",
      ""try"",
      ""verify"",
      ""deny"",
      ""defy"",
      ""rely"",
      ""modify"",
      ""satisfy"",
      ""spy""
    ],
    ""soundsLike"": [
      ""crew"",
      ""crime""
    ]
  },
  {
    ""word"": ""crystal"",
    ""rhymes"": [
      ""pistol""
    ],
    ""soundsLike"": []
  },
  {
    ""word"": ""cube"",
    ""rhymes"": [
      ""tube""
    ],
    ""soundsLike"": [
      ""club""
    ]
  },
  {
    ""word"": ""culture"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""color""
    ]
  },
  {
    ""word"": ""cup"",
    ""rhymes"": [
      ""setup""
    ],
    ""soundsLike"": [
      ""keep""
    ]
  },
  {
    ""word"": ""cupboard"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""card"",
      ""cart""
    ]
  },
  {
    ""word"": ""curious"",
    ""rhymes"": [],
    ""soundsLike"": []
  },
  {
    ""word"": ""current"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""grunt"",
      ""crunch"",
      ""grant"",
      ""crane""
    ]
  },
  {
    ""word"": ""curtain"",
    ""rhymes"": [
      ""certain""
    ],
    ""soundsLike"": [
      ""kitten"",
      ""cotton"",
      ""crane""
    ]
  },
  {
    ""word"": ""curve"",
    ""rhymes"": [
      ""nerve"",
      ""observe""
    ],
    ""soundsLike"": [
      ""cry"",
      ""cave""
    ]
  },
  {
    ""word"": ""cushion"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""caution"",
      ""kitchen"",
      ""cousin"",
      ""cotton""
    ]
  },
  {
    ""word"": ""custom"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""kitten""
    ]
  },
  {
    ""word"": ""cute"",
    ""rhymes"": [
      ""suit"",
      ""shoot"",
      ""route"",
      ""fruit"",
      ""minute"",
      ""execute"",
      ""salute"",
      ""lawsuit""
    ],
    ""soundsLike"": [
      ""quit"",
      ""quote"",
      ""cat"",
      ""caught""
    ]
  },
  {
    ""word"": ""cycle"",
    ""rhymes"": [
      ""recycle""
    ],
    ""soundsLike"": [
      ""circle"",
      ""skull"",
      ""skill"",
      ""scale""
    ]
  },
  {
    ""word"": ""dad"",
    ""rhymes"": [
      ""sad"",
      ""add"",
      ""mad"",
      ""glad""
    ],
    ""soundsLike"": []
  },
  {
    ""word"": ""damage"",
    ""rhymes"": [],
    ""soundsLike"": []
  },
  {
    ""word"": ""damp"",
    ""rhymes"": [
      ""camp"",
      ""lamp"",
      ""stamp"",
      ""ramp""
    ],
    ""soundsLike"": []
  },
  {
    ""word"": ""dance"",
    ""rhymes"": [
      ""enhance"",
      ""advance"",
      ""glance"",
      ""romance""
    ],
    ""soundsLike"": []
  },
  {
    ""word"": ""danger"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""dinner"",
      ""donor""
    ]
  },
  {
    ""word"": ""daring"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""during""
    ]
  },
  {
    ""word"": ""dash"",
    ""rhymes"": [
      ""flash"",
      ""crash"",
      ""trash"",
      ""cash""
    ],
    ""soundsLike"": [
      ""dish"",
      ""dutch""
    ]
  },
  {
    ""word"": ""daughter"",
    ""rhymes"": [
      ""water""
    ],
    ""soundsLike"": [
      ""door"",
      ""doctor""
    ]
  },
  {
    ""word"": ""dawn"",
    ""rhymes"": [
      ""upon"",
      ""spawn"",
      ""salon"",
      ""lawn""
    ],
    ""soundsLike"": [
      ""dune""
    ]
  },
  {
    ""word"": ""day"",
    ""rhymes"": [
      ""away"",
      ""way"",
      ""play"",
      ""say"",
      ""survey"",
      ""display"",
      ""stay"",
      ""delay"",
      ""holiday"",
      ""essay"",
      ""clay"",
      ""betray"",
      ""spray"",
      ""they"",
      ""okay"",
      ""tray"",
      ""obey"",
      ""today""
    ],
    ""soundsLike"": []
  },
  {
    ""word"": ""deal"",
    ""rhymes"": [
      ""wheel"",
      ""feel"",
      ""steel"",
      ""real"",
      ""reveal""
    ],
    ""soundsLike"": [
      ""doll"",
      ""dial""
    ]
  },
  {
    ""word"": ""debate"",
    ""rhymes"": [
      ""state"",
      ""rate"",
      ""plate"",
      ""great"",
      ""mandate"",
      ""estate"",
      ""gate"",
      ""indicate"",
      ""update"",
      ""wait""
    ],
    ""soundsLike"": []
  },
  {
    ""word"": ""debris"",
    ""rhymes"": [
      ""tree"",
      ""sea"",
      ""key"",
      ""flee"",
      ""degree"",
      ""agree"",
      ""fee"",
      ""ski"",
      ""three"",
      ""pony"",
      ""knee""
    ],
    ""soundsLike"": [
      ""diary""
    ]
  },
  {
    ""word"": ""decade"",
    ""rhymes"": [
      ""trade"",
      ""blade"",
      ""parade"",
      ""fade"",
      ""afraid"",
      ""upgrade"",
      ""maid""
    ],
    ""soundsLike"": [
      ""decorate""
    ]
  },
  {
    ""word"": ""december"",
    ""rhymes"": [
      ""member"",
      ""remember""
    ],
    ""soundsLike"": [
      ""timber"",
      ""despair""
    ]
  },
  {
    ""word"": ""decide"",
    ""rhymes"": [
      ""side"",
      ""provide"",
      ""ride"",
      ""slide"",
      ""pride"",
      ""tide"",
      ""guide"",
      ""divide"",
      ""wide"",
      ""glide"",
      ""inside"",
      ""outside""
    ],
    ""soundsLike"": [
      ""divide"",
      ""dust""
    ]
  },
  {
    ""word"": ""decline"",
    ""rhymes"": [
      ""design"",
      ""sign"",
      ""fine"",
      ""wine"",
      ""genuine"",
      ""shine"",
      ""define"",
      ""online"",
      ""combine""
    ],
    ""soundsLike"": []
  },
  {
    ""word"": ""decorate"",
    ""rhymes"": [
      ""state"",
      ""rate"",
      ""plate"",
      ""great"",
      ""mandate"",
      ""estate"",
      ""gate"",
      ""indicate"",
      ""update"",
      ""wait"",
      ""debate""
    ],
    ""soundsLike"": [
      ""decade""
    ]
  },
  {
    ""word"": ""decrease"",
    ""rhymes"": [
      ""piece"",
      ""peace"",
      ""increase"",
      ""release"",
      ""nice"",
      ""police""
    ],
    ""soundsLike"": [
      ""degree"",
      ""increase""
    ]
  },
  {
    ""word"": ""deer"",
    ""rhymes"": [
      ""year"",
      ""pioneer"",
      ""appear"",
      ""near"",
      ""sphere""
    ],
    ""soundsLike"": [
      ""door"",
      ""dinner"",
      ""dish"",
      ""differ""
    ]
  },
  {
    ""word"": ""defense"",
    ""rhymes"": [
      ""sense"",
      ""fence"",
      ""immense""
    ],
    ""soundsLike"": [
      ""define""
    ]
  },
  {
    ""word"": ""define"",
    ""rhymes"": [
      ""design"",
      ""sign"",
      ""fine"",
      ""wine"",
      ""genuine"",
      ""shine"",
      ""decline"",
      ""online"",
      ""combine""
    ],
    ""soundsLike"": [
      ""defy"",
      ""defense""
    ]
  },
  {
    ""word"": ""defy"",
    ""rhymes"": [
      ""fly"",
      ""eye"",
      ""high"",
      ""supply"",
      ""identify"",
      ""dry"",
      ""shy"",
      ""cry"",
      ""clarify"",
      ""try"",
      ""verify"",
      ""deny"",
      ""rely"",
      ""modify"",
      ""satisfy"",
      ""spy""
    ],
    ""soundsLike"": [
      ""define"",
      ""differ""
    ]
  },
  {
    ""word"": ""degree"",
    ""rhymes"": [
      ""tree"",
      ""sea"",
      ""key"",
      ""flee"",
      ""debris"",
      ""agree"",
      ""fee"",
      ""ski"",
      ""three"",
      ""pony"",
      ""knee""
    ],
    ""soundsLike"": [
      ""diary"",
      ""decrease"",
      ""debris""
    ]
  },
  {
    ""word"": ""delay"",
    ""rhymes"": [
      ""day"",
      ""away"",
      ""way"",
      ""play"",
      ""say"",
      ""survey"",
      ""display"",
      ""stay"",
      ""holiday"",
      ""essay"",
      ""clay"",
      ""betray"",
      ""spray"",
      ""they"",
      ""okay"",
      ""tray"",
      ""obey"",
      ""today""
    ],
    ""soundsLike"": []
  },
  {
    ""word"": ""deliver"",
    ""rhymes"": [
      ""river"",
      ""shiver""
    ],
    ""soundsLike"": [
      ""delay""
    ]
  },
  {
    ""word"": ""demand"",
    ""rhymes"": [
      ""hand"",
      ""stand"",
      ""brand"",
      ""expand"",
      ""sand""
    ],
    ""soundsLike"": [
      ""diamond""
    ]
  },
  {
    ""word"": ""demise"",
    ""rhymes"": [
      ""exercise"",
      ""wise"",
      ""size"",
      ""surprise"",
      ""prize""
    ],
    ""soundsLike"": [
      ""deny""
    ]
  },
  {
    ""word"": ""denial"",
    ""rhymes"": [
      ""trial"",
      ""aisle"",
      ""dial""
    ],
    ""soundsLike"": [
      ""deny"",
      ""tunnel"",
      ""dial""
    ]
  },
  {
    ""word"": ""dentist"",
    ""rhymes"": [],
    ""soundsLike"": []
  },
  {
    ""word"": ""deny"",
    ""rhymes"": [
      ""fly"",
      ""eye"",
      ""high"",
      ""supply"",
      ""identify"",
      ""dry"",
      ""shy"",
      ""cry"",
      ""clarify"",
      ""try"",
      ""verify"",
      ""defy"",
      ""rely"",
      ""modify"",
      ""satisfy"",
      ""spy""
    ],
    ""soundsLike"": [
      ""dinner""
    ]
  },
  {
    ""word"": ""depart"",
    ""rhymes"": [
      ""art"",
      ""smart"",
      ""heart"",
      ""start"",
      ""apart"",
      ""cart""
    ],
    ""soundsLike"": [
      ""desert"",
      ""report"",
      ""deposit"",
      ""divert"",
      ""apart"",
      ""debate""
    ]
  },
  {
    ""word"": ""depend"",
    ""rhymes"": [
      ""end"",
      ""friend"",
      ""lend"",
      ""attend"",
      ""trend"",
      ""extend"",
      ""spend"",
      ""weekend""
    ],
    ""soundsLike"": []
  },
  {
    ""word"": ""deposit"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""depart"",
      ""debate""
    ]
  },
  {
    ""word"": ""depth"",
    ""rhymes"": [],
    ""soundsLike"": []
  },
  {
    ""word"": ""deputy"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""duty""
    ]
  },
  {
    ""word"": ""derive"",
    ""rhymes"": [
      ""drive"",
      ""live"",
      ""thrive"",
      ""arrive""
    ],
    ""soundsLike"": [
      ""drive"",
      ""dry"",
      ""dove"",
      ""draw"",
      ""during"",
      ""thrive""
    ]
  },
  {
    ""word"": ""describe"",
    ""rhymes"": [
      ""tribe""
    ],
    ""soundsLike"": [
      ""degree"",
      ""destroy"",
      ""disagree""
    ]
  },
  {
    ""word"": ""desert"",
    ""rhymes"": [
      ""hurt"",
      ""alert"",
      ""skirt"",
      ""concert"",
      ""divert"",
      ""dirt""
    ],
    ""soundsLike"": [
      ""dust"",
      ""divert""
    ]
  },
  {
    ""word"": ""design"",
    ""rhymes"": [
      ""sign"",
      ""fine"",
      ""wine"",
      ""genuine"",
      ""shine"",
      ""decline"",
      ""define"",
      ""online"",
      ""combine""
    ],
    ""soundsLike"": [
      ""dizzy""
    ]
  },
  {
    ""word"": ""desk"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""task"",
      ""duck""
    ]
  },
  {
    ""word"": ""despair"",
    ""rhymes"": [
      ""air"",
      ""chair"",
      ""wear"",
      ""square"",
      ""share"",
      ""there"",
      ""hair"",
      ""spare"",
      ""aware"",
      ""pair"",
      ""where"",
      ""rare"",
      ""swear"",
      ""pear"",
      ""affair"",
      ""repair"",
      ""prepare"",
      ""glare"",
      ""scare"",
      ""warfare"",
      ""unfair"",
      ""unaware""
    ],
    ""soundsLike"": []
  },
  {
    ""word"": ""destroy"",
    ""rhymes"": [
      ""employ"",
      ""boy"",
      ""joy"",
      ""enjoy"",
      ""toy""
    ],
    ""soundsLike"": [
      ""history"",
      ""dust""
    ]
  },
  {
    ""word"": ""detail"",
    ""rhymes"": [
      ""scale"",
      ""rail"",
      ""mail"",
      ""whale"",
      ""tail"",
      ""sail"",
      ""female"",
      ""unveil"",
      ""inhale""
    ],
    ""soundsLike"": [
      ""title"",
      ""total""
    ]
  },
  {
    ""word"": ""detect"",
    ""rhymes"": [
      ""project"",
      ""subject"",
      ""aspect"",
      ""object"",
      ""perfect"",
      ""reflect"",
      ""direct"",
      ""connect"",
      ""expect"",
      ""correct"",
      ""neglect"",
      ""suspect"",
      ""collect"",
      ""select"",
      ""protect"",
      ""reject"",
      ""insect"",
      ""inject""
    ],
    ""soundsLike"": [
      ""direct""
    ]
  },
  {
    ""word"": ""develop"",
    ""rhymes"": [],
    ""soundsLike"": []
  },
  {
    ""word"": ""device"",
    ""rhymes"": [
      ""ice"",
      ""advice"",
      ""nice"",
      ""price"",
      ""rice"",
      ""dice"",
      ""spice"",
      ""slice"",
      ""twice""
    ],
    ""soundsLike"": [
      ""divide"",
      ""divorce"",
      ""advice"",
      ""defy""
    ]
  },
  {
    ""word"": ""devote"",
    ""rhymes"": [
      ""note"",
      ""boat"",
      ""promote"",
      ""float"",
      ""quote"",
      ""goat"",
      ""vote""
    ],
    ""soundsLike"": [
      ""divide"",
      ""divert"",
      ""device""
    ]
  },
  {
    ""word"": ""diagram"",
    ""rhymes"": [
      ""program"",
      ""slam"",
      ""cram""
    ],
    ""soundsLike"": [
      ""degree"",
      ""disagree""
    ]
  },
  {
    ""word"": ""dial"",
    ""rhymes"": [
      ""style"",
      ""file"",
      ""trial"",
      ""aisle"",
      ""smile"",
      ""exile"",
      ""denial""
    ],
    ""soundsLike"": [
      ""drill""
    ]
  },
  {
    ""word"": ""diamond"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""demand"",
      ""domain""
    ]
  },
  {
    ""word"": ""diary"",
    ""rhymes"": [
      ""inquiry""
    ],
    ""soundsLike"": []
  },
  {
    ""word"": ""dice"",
    ""rhymes"": [
      ""ice"",
      ""advice"",
      ""device"",
      ""nice"",
      ""price"",
      ""rice"",
      ""spice"",
      ""slice"",
      ""twice""
    ],
    ""soundsLike"": [
      ""dose""
    ]
  },
  {
    ""word"": ""diesel"",
    ""rhymes"": [
      ""weasel""
    ],
    ""soundsLike"": [
      ""dial""
    ]
  },
  {
    ""word"": ""diet"",
    ""rhymes"": [
      ""riot""
    ],
    ""soundsLike"": []
  },
  {
    ""word"": ""differ"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""deer"",
      ""defy""
    ]
  },
  {
    ""word"": ""digital"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""detail"",
      ""denial""
    ]
  },
  {
    ""word"": ""dignity"",
    ""rhymes"": [],
    ""soundsLike"": []
  },
  {
    ""word"": ""dilemma"",
    ""rhymes"": [],
    ""soundsLike"": []
  },
  {
    ""word"": ""dinner"",
    ""rhymes"": [
      ""inner"",
      ""winner""
    ],
    ""soundsLike"": [
      ""donor"",
      ""deny"",
      ""deer""
    ]
  },
  {
    ""word"": ""dinosaur"",
    ""rhymes"": [
      ""door"",
      ""more"",
      ""core"",
      ""floor"",
      ""before"",
      ""ignore"",
      ""outdoor"",
      ""indoor""
    ],
    ""soundsLike"": [
      ""dentist""
    ]
  },
  {
    ""word"": ""direct"",
    ""rhymes"": [
      ""project"",
      ""subject"",
      ""aspect"",
      ""object"",
      ""perfect"",
      ""reflect"",
      ""connect"",
      ""expect"",
      ""correct"",
      ""neglect"",
      ""suspect"",
      ""collect"",
      ""select"",
      ""protect"",
      ""reject"",
      ""detect"",
      ""insect"",
      ""inject""
    ],
    ""soundsLike"": [
      ""drift"",
      ""correct""
    ]
  },
  {
    ""word"": ""dirt"",
    ""rhymes"": [
      ""desert"",
      ""hurt"",
      ""alert"",
      ""skirt"",
      ""concert"",
      ""divert""
    ],
    ""soundsLike"": [
      ""dry"",
      ""draw"",
      ""art""
    ]
  },
  {
    ""word"": ""disagree"",
    ""rhymes"": [
      ""tree"",
      ""sea"",
      ""key"",
      ""flee"",
      ""degree"",
      ""debris"",
      ""agree"",
      ""fee"",
      ""ski"",
      ""three"",
      ""pony"",
      ""knee""
    ],
    ""soundsLike"": [
      ""degree"",
      ""destroy"",
      ""describe"",
      ""diagram""
    ]
  },
  {
    ""word"": ""discover"",
    ""rhymes"": [
      ""cover"",
      ""hover"",
      ""uncover""
    ],
    ""soundsLike"": [
      ""deliver""
    ]
  },
  {
    ""word"": ""disease"",
    ""rhymes"": [
      ""cheese"",
      ""squeeze"",
      ""breeze"",
      ""please""
    ],
    ""soundsLike"": [
      ""dizzy""
    ]
  },
  {
    ""word"": ""dish"",
    ""rhymes"": [
      ""fish"",
      ""wish""
    ],
    ""soundsLike"": [
      ""dash"",
      ""dutch"",
      ""deer""
    ]
  },
  {
    ""word"": ""dismiss"",
    ""rhymes"": [
      ""miss"",
      ""this"",
      ""kiss""
    ],
    ""soundsLike"": [
      ""demise""
    ]
  },
  {
    ""word"": ""disorder"",
    ""rhymes"": [
      ""order"",
      ""border""
    ],
    ""soundsLike"": [
      ""decide""
    ]
  },
  {
    ""word"": ""display"",
    ""rhymes"": [
      ""day"",
      ""away"",
      ""way"",
      ""play"",
      ""say"",
      ""survey"",
      ""stay"",
      ""delay"",
      ""holiday"",
      ""essay"",
      ""clay"",
      ""betray"",
      ""spray"",
      ""they"",
      ""okay"",
      ""tray"",
      ""obey"",
      ""today""
    ],
    ""soundsLike"": [
      ""delay"",
      ""despair""
    ]
  },
  {
    ""word"": ""distance"",
    ""rhymes"": [],
    ""soundsLike"": []
  },
  {
    ""word"": ""divert"",
    ""rhymes"": [
      ""desert"",
      ""hurt"",
      ""alert"",
      ""skirt"",
      ""concert"",
      ""dirt""
    ],
    ""soundsLike"": [
      ""dirt"",
      ""desert"",
      ""diet"",
      ""divide"",
      ""diary"",
      ""devote""
    ]
  },
  {
    ""word"": ""divide"",
    ""rhymes"": [
      ""side"",
      ""provide"",
      ""ride"",
      ""slide"",
      ""pride"",
      ""tide"",
      ""guide"",
      ""decide"",
      ""wide"",
      ""glide"",
      ""inside"",
      ""outside""
    ],
    ""soundsLike"": [
      ""devote"",
      ""decide"",
      ""device"",
      ""divert"",
      ""defy""
    ]
  },
  {
    ""word"": ""divorce"",
    ""rhymes"": [
      ""force"",
      ""course"",
      ""horse"",
      ""source"",
      ""endorse"",
      ""enforce""
    ],
    ""soundsLike"": [
      ""device""
    ]
  },
  {
    ""word"": ""dizzy"",
    ""rhymes"": [
      ""busy""
    ],
    ""soundsLike"": [
      ""disease""
    ]
  },
  {
    ""word"": ""doctor"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""daughter""
    ]
  },
  {
    ""word"": ""document"",
    ""rhymes"": [],
    ""soundsLike"": []
  },
  {
    ""word"": ""dog"",
    ""rhymes"": [
      ""frog"",
      ""fog"",
      ""hedgehog"",
      ""clog""
    ],
    ""soundsLike"": [
      ""duck""
    ]
  },
  {
    ""word"": ""doll"",
    ""rhymes"": [
      ""all"",
      ""ball"",
      ""call"",
      ""fall"",
      ""wall"",
      ""recall"",
      ""small"",
      ""alcohol"",
      ""install"",
      ""crawl""
    ],
    ""soundsLike"": [
      ""deal"",
      ""dial""
    ]
  },
  {
    ""word"": ""dolphin"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""define""
    ]
  },
  {
    ""word"": ""domain"",
    ""rhymes"": [
      ""train"",
      ""sustain"",
      ""rain"",
      ""again"",
      ""grain"",
      ""obtain"",
      ""brain"",
      ""gain"",
      ""explain"",
      ""main"",
      ""crane"",
      ""insane"",
      ""remain""
    ],
    ""soundsLike"": []
  },
  {
    ""word"": ""donate"",
    ""rhymes"": [
      ""state"",
      ""rate"",
      ""plate"",
      ""great"",
      ""mandate"",
      ""estate"",
      ""gate"",
      ""indicate"",
      ""update"",
      ""wait"",
      ""debate""
    ],
    ""soundsLike"": [
      ""tonight""
    ]
  },
  {
    ""word"": ""donkey"",
    ""rhymes"": [],
    ""soundsLike"": []
  },
  {
    ""word"": ""donor"",
    ""rhymes"": [
      ""owner""
    ],
    ""soundsLike"": [
      ""dinner"",
      ""danger""
    ]
  },
  {
    ""word"": ""door"",
    ""rhymes"": [
      ""more"",
      ""core"",
      ""floor"",
      ""before"",
      ""ignore"",
      ""dinosaur"",
      ""outdoor"",
      ""indoor""
    ],
    ""soundsLike"": [
      ""deer"",
      ""daughter""
    ]
  },
  {
    ""word"": ""dose"",
    ""rhymes"": [
      ""close""
    ],
    ""soundsLike"": [
      ""dice""
    ]
  },
  {
    ""word"": ""double"",
    ""rhymes"": [
      ""bubble"",
      ""trouble""
    ],
    ""soundsLike"": [
      ""table"",
      ""dial""
    ]
  },
  {
    ""word"": ""dove"",
    ""rhymes"": [
      ""love"",
      ""above"",
      ""shove"",
      ""stove"",
      ""glove""
    ],
    ""soundsLike"": [
      ""dutch""
    ]
  },
  {
    ""word"": ""draft"",
    ""rhymes"": [
      ""craft"",
      ""shaft""
    ],
    ""soundsLike"": [
      ""drift"",
      ""dirt"",
      ""craft"",
      ""direct""
    ]
  },
  {
    ""word"": ""dragon"",
    ""rhymes"": [
      ""wagon""
    ],
    ""soundsLike"": []
  },
  {
    ""word"": ""drama"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""dream"",
      ""drum""
    ]
  },
  {
    ""word"": ""drastic"",
    ""rhymes"": [
      ""plastic""
    ],
    ""soundsLike"": []
  },
  {
    ""word"": ""draw"",
    ""rhymes"": [
      ""law"",
      ""raw"",
      ""claw""
    ],
    ""soundsLike"": [
      ""dry"",
      ""dirt"",
      ""drop"",
      ""tree"",
      ""tray""
    ]
  },
  {
    ""word"": ""dream"",
    ""rhymes"": [
      ""cream"",
      ""scheme"",
      ""team"",
      ""theme"",
      ""supreme""
    ],
    ""soundsLike"": [
      ""drum"",
      ""during"",
      ""trim""
    ]
  },
  {
    ""word"": ""dress"",
    ""rhymes"": [
      ""process"",
      ""address"",
      ""access"",
      ""express"",
      ""success"",
      ""guess"",
      ""excess"",
      ""bless""
    ],
    ""soundsLike"": [
      ""address""
    ]
  },
  {
    ""word"": ""drift"",
    ""rhymes"": [
      ""shift"",
      ""lift"",
      ""gift"",
      ""swift""
    ],
    ""soundsLike"": [
      ""draft"",
      ""direct"",
      ""dirt"",
      ""defy""
    ]
  },
  {
    ""word"": ""drill"",
    ""rhymes"": [
      ""will"",
      ""still"",
      ""hill"",
      ""skill"",
      ""pill"",
      ""ill"",
      ""until""
    ],
    ""soundsLike"": [
      ""during"",
      ""trial"",
      ""dial"",
      ""deal""
    ]
  },
  {
    ""word"": ""drink"",
    ""rhymes"": [
      ""link"",
      ""pink"",
      ""wink""
    ],
    ""soundsLike"": [
      ""during"",
      ""trick"",
      ""drum"",
      ""drill"",
      ""daring""
    ]
  },
  {
    ""word"": ""drip"",
    ""rhymes"": [
      ""ship"",
      ""trip"",
      ""tip"",
      ""clip"",
      ""flip"",
      ""whip"",
      ""hip"",
      ""equip""
    ],
    ""soundsLike"": [
      ""drop"",
      ""trip"",
      ""trap"",
      ""draw""
    ]
  },
  {
    ""word"": ""drive"",
    ""rhymes"": [
      ""live"",
      ""thrive"",
      ""derive"",
      ""arrive""
    ],
    ""soundsLike"": [
      ""derive"",
      ""dry"",
      ""dove"",
      ""draw"",
      ""during"",
      ""thrive""
    ]
  },
  {
    ""word"": ""drop"",
    ""rhymes"": [
      ""top"",
      ""shop"",
      ""crop"",
      ""swap"",
      ""laptop""
    ],
    ""soundsLike"": [
      ""drip"",
      ""trip"",
      ""trap"",
      ""draw""
    ]
  },
  {
    ""word"": ""drum"",
    ""rhymes"": [
      ""become"",
      ""come"",
      ""income"",
      ""thumb"",
      ""dumb""
    ],
    ""soundsLike"": [
      ""dream"",
      ""during"",
      ""dumb"",
      ""trim""
    ]
  },
  {
    ""word"": ""dry"",
    ""rhymes"": [
      ""fly"",
      ""eye"",
      ""high"",
      ""supply"",
      ""identify"",
      ""shy"",
      ""cry"",
      ""clarify"",
      ""try"",
      ""verify"",
      ""deny"",
      ""defy"",
      ""rely"",
      ""modify"",
      ""satisfy"",
      ""spy""
    ],
    ""soundsLike"": [
      ""draw"",
      ""try"",
      ""dirt"",
      ""drive"",
      ""derive""
    ]
  },
  {
    ""word"": ""duck"",
    ""rhymes"": [
      ""truck"",
      ""pluck""
    ],
    ""soundsLike"": [
      ""dog""
    ]
  },
  {
    ""word"": ""dumb"",
    ""rhymes"": [
      ""become"",
      ""come"",
      ""income"",
      ""thumb"",
      ""drum""
    ],
    ""soundsLike"": [
      ""dawn""
    ]
  },
  {
    ""word"": ""dune"",
    ""rhymes"": [
      ""moon"",
      ""spoon"",
      ""soon"",
      ""immune"",
      ""raccoon""
    ],
    ""soundsLike"": [
      ""dawn""
    ]
  },
  {
    ""word"": ""during"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""daring""
    ]
  },
  {
    ""word"": ""dust"",
    ""rhymes"": [
      ""trust"",
      ""robust"",
      ""just"",
      ""must"",
      ""adjust""
    ],
    ""soundsLike"": [
      ""test"",
      ""taste"",
      ""toast""
    ]
  },
  {
    ""word"": ""dutch"",
    ""rhymes"": [
      ""such"",
      ""clutch"",
      ""much""
    ],
    ""soundsLike"": [
      ""dish"",
      ""duck"",
      ""dove"",
      ""dash"",
      ""teach""
    ]
  },
  {
    ""word"": ""duty"",
    ""rhymes"": [
      ""beauty""
    ],
    ""soundsLike"": []
  },
  {
    ""word"": ""dwarf"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""door"",
      ""deer""
    ]
  },
  {
    ""word"": ""dynamic"",
    ""rhymes"": [],
    ""soundsLike"": []
  },
  {
    ""word"": ""eager"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""occur""
    ]
  },
  {
    ""word"": ""eagle"",
    ""rhymes"": [
      ""legal"",
      ""illegal""
    ],
    ""soundsLike"": [
      ""legal""
    ]
  },
  {
    ""word"": ""early"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""rally""
    ]
  },
  {
    ""word"": ""earn"",
    ""rhymes"": [
      ""turn"",
      ""learn"",
      ""return"",
      ""churn""
    ],
    ""soundsLike"": [
      ""around"",
      ""iron"",
      ""churn""
    ]
  },
  {
    ""word"": ""earth"",
    ""rhymes"": [
      ""worth"",
      ""birth""
    ],
    ""soundsLike"": [
      ""raw"",
      ""birth""
    ]
  },
  {
    ""word"": ""easily"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""easy"",
      ""diesel""
    ]
  },
  {
    ""word"": ""east"",
    ""rhymes"": [],
    ""soundsLike"": []
  },
  {
    ""word"": ""easy"",
    ""rhymes"": [],
    ""soundsLike"": []
  },
  {
    ""word"": ""echo"",
    ""rhymes"": [],
    ""soundsLike"": []
  },
  {
    ""word"": ""ecology"",
    ""rhymes"": [
      ""biology"",
      ""apology""
    ],
    ""soundsLike"": [
      ""apology"",
      ""biology""
    ]
  },
  {
    ""word"": ""economy"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""enemy"",
      ""ecology""
    ]
  },
  {
    ""word"": ""edge"",
    ""rhymes"": [
      ""pledge""
    ],
    ""soundsLike"": [
      ""age"",
      ""egg""
    ]
  },
  {
    ""word"": ""edit"",
    ""rhymes"": [
      ""credit""
    ],
    ""soundsLike"": [
      ""audit"",
      ""adult""
    ]
  },
  {
    ""word"": ""educate"",
    ""rhymes"": [
      ""state"",
      ""rate"",
      ""plate"",
      ""great"",
      ""mandate"",
      ""estate"",
      ""gate"",
      ""indicate"",
      ""update"",
      ""wait"",
      ""debate""
    ],
    ""soundsLike"": [
      ""adjust"",
      ""agent"",
      ""object"",
      ""reject"",
      ""inject""
    ]
  },
  {
    ""word"": ""effort"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""art"",
      ""offer""
    ]
  },
  {
    ""word"": ""egg"",
    ""rhymes"": [
      ""leg""
    ],
    ""soundsLike"": [
      ""edge""
    ]
  },
  {
    ""word"": ""eight"",
    ""rhymes"": [
      ""state"",
      ""rate"",
      ""plate"",
      ""great"",
      ""mandate"",
      ""estate"",
      ""gate"",
      ""indicate"",
      ""update"",
      ""wait"",
      ""debate""
    ],
    ""soundsLike"": []
  },
  {
    ""word"": ""either"",
    ""rhymes"": [
      ""neither""
    ],
    ""soundsLike"": [
      ""other"",
      ""author"",
      ""neither"",
      ""eager"",
      ""offer"",
      ""odor""
    ]
  },
  {
    ""word"": ""elbow"",
    ""rhymes"": [
      ""know"",
      ""snow"",
      ""throw"",
      ""hello"",
      ""grow"",
      ""shadow"",
      ""potato"",
      ""toe"",
      ""glow"",
      ""slow"",
      ""tomato"",
      ""below"",
      ""radio"",
      ""meadow"",
      ""tomorrow"",
      ""borrow"",
      ""photo"",
      ""tornado"",
      ""buffalo""
    ],
    ""soundsLike"": []
  },
  {
    ""word"": ""elder"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""alter""
    ]
  },
  {
    ""word"": ""electric"",
    ""rhymes"": [],
    ""soundsLike"": []
  },
  {
    ""word"": ""elegant"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""elephant"",
      ""element"",
      ""pelican"",
      ""client"",
      ""slogan""
    ]
  },
  {
    ""word"": ""element"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""elephant"",
      ""elegant"",
      ""cement""
    ]
  },
  {
    ""word"": ""elephant"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""elegant"",
      ""element"",
      ""infant"",
      ""client""
    ]
  },
  {
    ""word"": ""elevator"",
    ""rhymes"": [
      ""later"",
      ""crater""
    ],
    ""soundsLike"": []
  },
  {
    ""word"": ""elite"",
    ""rhymes"": [
      ""seat"",
      ""treat"",
      ""street"",
      ""sweet"",
      ""meat"",
      ""retreat"",
      ""athlete"",
      ""wheat"",
      ""repeat""
    ],
    ""soundsLike"": [
      ""alert""
    ]
  },
  {
    ""word"": ""else"",
    ""rhymes"": [],
    ""soundsLike"": []
  },
  {
    ""word"": ""embark"",
    ""rhymes"": [
      ""park""
    ],
    ""soundsLike"": [
      ""empower""
    ]
  },
  {
    ""word"": ""embody"",
    ""rhymes"": [
      ""body""
    ],
    ""soundsLike"": [
      ""empty"",
      ""embark""
    ]
  },
  {
    ""word"": ""embrace"",
    ""rhymes"": [
      ""case"",
      ""grace"",
      ""face"",
      ""base"",
      ""place"",
      ""space"",
      ""race"",
      ""chase"",
      ""replace"",
      ""erase""
    ],
    ""soundsLike"": []
  },
  {
    ""word"": ""emerge"",
    ""rhymes"": [
      ""surge"",
      ""urge"",
      ""merge""
    ],
    ""soundsLike"": [
      ""image"",
      ""inner""
    ]
  },
  {
    ""word"": ""emotion"",
    ""rhymes"": [
      ""motion"",
      ""ocean""
    ],
    ""soundsLike"": [
      ""motion"",
      ""engine""
    ]
  },
  {
    ""word"": ""employ"",
    ""rhymes"": [
      ""boy"",
      ""joy"",
      ""enjoy"",
      ""destroy"",
      ""toy""
    ],
    ""soundsLike"": [
      ""empty""
    ]
  },
  {
    ""word"": ""empower"",
    ""rhymes"": [
      ""power"",
      ""flower"",
      ""hour"",
      ""tower""
    ],
    ""soundsLike"": [
      ""impose"",
      ""embark"",
      ""appear""
    ]
  },
  {
    ""word"": ""empty"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""employ""
    ]
  },
  {
    ""word"": ""enable"",
    ""rhymes"": [
      ""table"",
      ""label"",
      ""able"",
      ""stable"",
      ""cable"",
      ""unable""
    ],
    ""soundsLike"": [
      ""unable"",
      ""table"",
      ""label"",
      ""cable""
    ]
  },
  {
    ""word"": ""enact"",
    ""rhymes"": [
      ""act"",
      ""impact"",
      ""abstract"",
      ""intact"",
      ""exact"",
      ""attract"",
      ""pact""
    ],
    ""soundsLike"": [
      ""intact"",
      ""impact"",
      ""insect"",
      ""inject"",
      ""connect""
    ]
  },
  {
    ""word"": ""end"",
    ""rhymes"": [
      ""friend"",
      ""lend"",
      ""attend"",
      ""trend"",
      ""extend"",
      ""spend"",
      ""depend"",
      ""weekend""
    ],
    ""soundsLike"": [
      ""any"",
      ""lend""
    ]
  },
  {
    ""word"": ""endless"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""endorse"",
      ""enlist""
    ]
  },
  {
    ""word"": ""endorse"",
    ""rhymes"": [
      ""force"",
      ""course"",
      ""horse"",
      ""source"",
      ""enforce"",
      ""divorce""
    ],
    ""soundsLike"": [
      ""indoor"",
      ""endless"",
      ""enforce""
    ]
  },
  {
    ""word"": ""enemy"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""any""
    ]
  },
  {
    ""word"": ""energy"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""enjoy"",
      ""enrich"",
      ""any"",
      ""enter"",
      ""emerge"",
      ""entry"",
      ""enroll""
    ]
  },
  {
    ""word"": ""enforce"",
    ""rhymes"": [
      ""force"",
      ""course"",
      ""horse"",
      ""source"",
      ""endorse"",
      ""divorce""
    ],
    ""soundsLike"": [
      ""unfair"",
      ""inform"",
      ""endorse""
    ]
  },
  {
    ""word"": ""engage"",
    ""rhymes"": [
      ""gauge"",
      ""age"",
      ""stage"",
      ""page"",
      ""cage"",
      ""wage""
    ],
    ""soundsLike"": [
      ""enjoy"",
      ""any""
    ]
  },
  {
    ""word"": ""engine"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""onion"",
      ""enjoy"",
      ""mention""
    ]
  },
  {
    ""word"": ""enhance"",
    ""rhymes"": [
      ""dance"",
      ""advance"",
      ""glance"",
      ""romance""
    ],
    ""soundsLike"": [
      ""announce"",
      ""sentence"",
      ""essence"",
      ""engine"",
      ""immense""
    ]
  },
  {
    ""word"": ""enjoy"",
    ""rhymes"": [
      ""employ"",
      ""boy"",
      ""joy"",
      ""destroy"",
      ""toy""
    ],
    ""soundsLike"": [
      ""energy"",
      ""any"",
      ""engine""
    ]
  },
  {
    ""word"": ""enlist"",
    ""rhymes"": [
      ""list"",
      ""just"",
      ""twist"",
      ""assist"",
      ""exist"",
      ""resist"",
      ""wrist""
    ],
    ""soundsLike"": [
      ""analyst"",
      ""dentist"",
      ""endless""
    ]
  },
  {
    ""word"": ""enough"",
    ""rhymes"": [
      ""stuff"",
      ""rough""
    ],
    ""soundsLike"": [
      ""sniff""
    ]
  },
  {
    ""word"": ""enrich"",
    ""rhymes"": [
      ""pitch"",
      ""switch"",
      ""rich""
    ],
    ""soundsLike"": [
      ""enroll"",
      ""energy"",
      ""enter""
    ]
  },
  {
    ""word"": ""enroll"",
    ""rhymes"": [
      ""control"",
      ""hole"",
      ""pole"",
      ""soul"",
      ""patrol""
    ],
    ""soundsLike"": [
      ""general"",
      ""enrich"",
      ""energy"",
      ""arrow""
    ]
  },
  {
    ""word"": ""ensure"",
    ""rhymes"": [
      ""obscure"",
      ""sure""
    ],
    ""soundsLike"": [
      ""enjoy"",
      ""enter"",
      ""entire""
    ]
  },
  {
    ""word"": ""enter"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""under"",
      ""entry"",
      ""end"",
      ""empty""
    ]
  },
  {
    ""word"": ""entire"",
    ""rhymes"": [
      ""fire"",
      ""wire"",
      ""acquire"",
      ""require"",
      ""hire"",
      ""liar"",
      ""retire"",
      ""buyer""
    ],
    ""soundsLike"": [
      ""indoor"",
      ""into"",
      ""enter"",
      ""ensure""
    ]
  },
  {
    ""word"": ""entry"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""enter"",
      ""country"",
      ""empty""
    ]
  },
  {
    ""word"": ""envelope"",
    ""rhymes"": [
      ""hope"",
      ""soap""
    ],
    ""soundsLike"": [
      ""involve"",
      ""unveil""
    ]
  },
  {
    ""word"": ""episode"",
    ""rhymes"": [
      ""code"",
      ""road"",
      ""load"",
      ""erode""
    ],
    ""soundsLike"": [
      ""upset""
    ]
  },
  {
    ""word"": ""equal"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""eagle"",
      ""actual""
    ]
  },
  {
    ""word"": ""equip"",
    ""rhymes"": [
      ""ship"",
      ""trip"",
      ""tip"",
      ""clip"",
      ""flip"",
      ""whip"",
      ""drip"",
      ""hip""
    ],
    ""soundsLike"": [
      ""whip"",
      ""escape""
    ]
  },
  {
    ""word"": ""era"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""arrow"",
      ""air"",
      ""area""
    ]
  },
  {
    ""word"": ""erase"",
    ""rhymes"": [
      ""case"",
      ""grace"",
      ""face"",
      ""base"",
      ""place"",
      ""space"",
      ""race"",
      ""embrace"",
      ""chase"",
      ""replace""
    ],
    ""soundsLike"": [
      ""grace"",
      ""era""
    ]
  },
  {
    ""word"": ""erode"",
    ""rhymes"": [
      ""code"",
      ""road"",
      ""load"",
      ""episode""
    ],
    ""soundsLike"": [
      ""arrow"",
      ""era""
    ]
  },
  {
    ""word"": ""erosion"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""frozen"",
      ""version""
    ]
  },
  {
    ""word"": ""error"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""air"",
      ""era""
    ]
  },
  {
    ""word"": ""erupt"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""era"",
      ""erode""
    ]
  },
  {
    ""word"": ""escape"",
    ""rhymes"": [
      ""tape"",
      ""grape""
    ],
    ""soundsLike"": []
  },
  {
    ""word"": ""essay"",
    ""rhymes"": [
      ""day"",
      ""away"",
      ""way"",
      ""play"",
      ""say"",
      ""survey"",
      ""display"",
      ""stay"",
      ""delay"",
      ""holiday"",
      ""clay"",
      ""betray"",
      ""spray"",
      ""they"",
      ""okay"",
      ""tray"",
      ""obey"",
      ""today""
    ],
    ""soundsLike"": []
  },
  {
    ""word"": ""essence"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""license""
    ]
  },
  {
    ""word"": ""estate"",
    ""rhymes"": [
      ""state"",
      ""rate"",
      ""plate"",
      ""great"",
      ""mandate"",
      ""gate"",
      ""indicate"",
      ""update"",
      ""wait"",
      ""debate""
    ],
    ""soundsLike"": [
      ""imitate""
    ]
  },
  {
    ""word"": ""eternal"",
    ""rhymes"": [],
    ""soundsLike"": []
  },
  {
    ""word"": ""ethics"",
    ""rhymes"": [],
    ""soundsLike"": []
  },
  {
    ""word"": ""evidence"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""essence""
    ]
  },
  {
    ""word"": ""evil"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""oval"",
      ""eagle"",
      ""reveal"",
      ""evolve"",
      ""awful"",
      ""aisle"",
      ""civil""
    ]
  },
  {
    ""word"": ""evoke"",
    ""rhymes"": [
      ""oak"",
      ""smoke"",
      ""joke""
    ],
    ""soundsLike"": [
      ""awake""
    ]
  },
  {
    ""word"": ""evolve"",
    ""rhymes"": [
      ""involve"",
      ""solve""
    ],
    ""soundsLike"": [
      ""involve"",
      ""evil"",
      ""solve"",
      ""oval"",
      ""reveal"",
      ""civil"",
      ""valve""
    ]
  },
  {
    ""word"": ""exact"",
    ""rhymes"": [
      ""act"",
      ""impact"",
      ""abstract"",
      ""enact"",
      ""intact"",
      ""attract"",
      ""pact""
    ],
    ""soundsLike"": [
      ""exist"",
      ""exit"",
      ""exhaust"",
      ""expect"",
      ""intact"",
      ""excite"",
      ""insect"",
      ""exhibit"",
      ""enact"",
      ""attract""
    ]
  },
  {
    ""word"": ""example"",
    ""rhymes"": [
      ""sample""
    ],
    ""soundsLike"": [
      ""resemble""
    ]
  },
  {
    ""word"": ""excess"",
    ""rhymes"": [
      ""process"",
      ""address"",
      ""access"",
      ""express"",
      ""success"",
      ""dress"",
      ""guess"",
      ""bless""
    ],
    ""soundsLike"": [
      ""access"",
      ""axis"",
      ""success"",
      ""exit""
    ]
  },
  {
    ""word"": ""exchange"",
    ""rhymes"": [
      ""change"",
      ""range"",
      ""arrange""
    ],
    ""soundsLike"": [
      ""explain"",
      ""extend""
    ]
  },
  {
    ""word"": ""excite"",
    ""rhymes"": [
      ""light"",
      ""right"",
      ""night"",
      ""write"",
      ""sight"",
      ""flight"",
      ""bright"",
      ""slight"",
      ""height"",
      ""kite"",
      ""invite"",
      ""midnight""
    ],
    ""soundsLike"": [
      ""exit"",
      ""exile"",
      ""excess""
    ]
  },
  {
    ""word"": ""exclude"",
    ""rhymes"": [
      ""food"",
      ""attitude"",
      ""rude"",
      ""include""
    ],
    ""soundsLike"": [
      ""excuse"",
      ""include"",
      ""excite"",
      ""execute""
    ]
  },
  {
    ""word"": ""excuse"",
    ""rhymes"": [
      ""choose"",
      ""use"",
      ""abuse"",
      ""produce"",
      ""goose"",
      ""news"",
      ""juice"",
      ""reduce"",
      ""refuse"",
      ""cruise"",
      ""accuse""
    ],
    ""soundsLike"": [
      ""excess"",
      ""exclude"",
      ""execute"",
      ""accuse""
    ]
  },
  {
    ""word"": ""execute"",
    ""rhymes"": [
      ""suit"",
      ""shoot"",
      ""route"",
      ""fruit"",
      ""minute"",
      ""cute"",
      ""salute"",
      ""lawsuit""
    ],
    ""soundsLike"": [
      ""excuse"",
      ""exit"",
      ""exclude""
    ]
  },
  {
    ""word"": ""exercise"",
    ""rhymes"": [
      ""demise"",
      ""wise"",
      ""size"",
      ""surprise"",
      ""prize""
    ],
    ""soundsLike"": [
      ""excess""
    ]
  },
  {
    ""word"": ""exhaust"",
    ""rhymes"": [
      ""cost"",
      ""frost""
    ],
    ""soundsLike"": [
      ""exist"",
      ""exit"",
      ""exact"",
      ""august"",
      ""resist"",
      ""assist"",
      ""excite"",
      ""excess"",
      ""exhibit"",
      ""adjust""
    ]
  },
  {
    ""word"": ""exhibit"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""exist"",
      ""exit"",
      ""exact"",
      ""exhaust""
    ]
  },
  {
    ""word"": ""exile"",
    ""rhymes"": [
      ""style"",
      ""file"",
      ""trial"",
      ""aisle"",
      ""smile"",
      ""dial""
    ],
    ""soundsLike"": [
      ""exit"",
      ""eagle""
    ]
  },
  {
    ""word"": ""exist"",
    ""rhymes"": [
      ""list"",
      ""just"",
      ""twist"",
      ""assist"",
      ""resist"",
      ""enlist"",
      ""wrist""
    ],
    ""soundsLike"": [
      ""exhaust"",
      ""exit"",
      ""resist"",
      ""august"",
      ""assist"",
      ""exact"",
      ""exhibit"",
      ""excess"",
      ""excite""
    ]
  },
  {
    ""word"": ""exit"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""exist"",
      ""exact"",
      ""excite"",
      ""exile"",
      ""exhaust"",
      ""visit""
    ]
  },
  {
    ""word"": ""exotic"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""exit"",
      ""acoustic""
    ]
  },
  {
    ""word"": ""expand"",
    ""rhymes"": [
      ""hand"",
      ""stand"",
      ""demand"",
      ""brand"",
      ""sand""
    ],
    ""soundsLike"": [
      ""extend"",
      ""explain"",
      ""expect"",
      ""excite""
    ]
  },
  {
    ""word"": ""expect"",
    ""rhymes"": [
      ""project"",
      ""subject"",
      ""aspect"",
      ""object"",
      ""perfect"",
      ""reflect"",
      ""direct"",
      ""connect"",
      ""correct"",
      ""neglect"",
      ""suspect"",
      ""collect"",
      ""select"",
      ""protect"",
      ""reject"",
      ""detect"",
      ""insect"",
      ""inject""
    ],
    ""soundsLike"": [
      ""aspect"",
      ""suspect"",
      ""exact"",
      ""expose"",
      ""excite"",
      ""exit"",
      ""impact"",
      ""expand"",
      ""insect"",
      ""expire""
    ]
  },
  {
    ""word"": ""expire"",
    ""rhymes"": [
      ""fire"",
      ""wire"",
      ""inspire"",
      ""require"",
      ""hire"",
      ""retire""
    ],
    ""soundsLike"": [
      ""expose"",
      ""inspire"",
      ""despair"",
      ""excite"",
      ""acquire""
    ]
  },
  {
    ""word"": ""explain"",
    ""rhymes"": [
      ""train"",
      ""domain"",
      ""sustain"",
      ""rain"",
      ""again"",
      ""grain"",
      ""obtain"",
      ""brain"",
      ""gain"",
      ""main"",
      ""crane"",
      ""insane"",
      ""remain""
    ],
    ""soundsLike"": [
      ""expand"",
      ""display"",
      ""exchange"",
      ""expose"",
      ""expire""
    ]
  },
  {
    ""word"": ""expose"",
    ""rhymes"": [
      ""rose"",
      ""close"",
      ""nose"",
      ""impose"",
      ""oppose""
    ],
    ""soundsLike"": [
      ""expire"",
      ""express"",
      ""excess"",
      ""expect"",
      ""excite"",
      ""oppose"",
      ""impose"",
      ""access""
    ]
  },
  {
    ""word"": ""express"",
    ""rhymes"": [
      ""process"",
      ""address"",
      ""access"",
      ""success"",
      ""dress"",
      ""guess"",
      ""excess"",
      ""bless""
    ],
    ""soundsLike"": [
      ""expose"",
      ""excess"",
      ""access"",
      ""across"",
      ""axis"",
      ""expire""
    ]
  },
  {
    ""word"": ""extend"",
    ""rhymes"": [
      ""end"",
      ""friend"",
      ""lend"",
      ""attend"",
      ""trend"",
      ""spend"",
      ""depend"",
      ""weekend""
    ],
    ""soundsLike"": [
      ""expand"",
      ""attend""
    ]
  },
  {
    ""word"": ""extra"",
    ""rhymes"": [],
    ""soundsLike"": []
  },
  {
    ""word"": ""eye"",
    ""rhymes"": [
      ""fly"",
      ""high"",
      ""supply"",
      ""identify"",
      ""dry"",
      ""shy"",
      ""cry"",
      ""clarify"",
      ""try"",
      ""verify"",
      ""deny"",
      ""defy"",
      ""rely"",
      ""modify"",
      ""satisfy"",
      ""spy""
    ],
    ""soundsLike"": []
  },
  {
    ""word"": ""eyebrow"",
    ""rhymes"": [
      ""now"",
      ""allow""
    ],
    ""soundsLike"": [
      ""opera""
    ]
  },
  {
    ""word"": ""fabric"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""fork"",
      ""fiber""
    ]
  },
  {
    ""word"": ""face"",
    ""rhymes"": [
      ""case"",
      ""grace"",
      ""base"",
      ""place"",
      ""space"",
      ""race"",
      ""embrace"",
      ""chase"",
      ""replace"",
      ""erase""
    ],
    ""soundsLike"": [
      ""faith""
    ]
  },
  {
    ""word"": ""faculty"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""family""
    ]
  },
  {
    ""word"": ""fade"",
    ""rhymes"": [
      ""trade"",
      ""blade"",
      ""parade"",
      ""afraid"",
      ""decade"",
      ""upgrade"",
      ""maid""
    ],
    ""soundsLike"": [
      ""food"",
      ""feed"",
      ""fit"",
      ""fat"",
      ""foot""
    ]
  },
  {
    ""word"": ""faint"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""find"",
      ""found"",
      ""fat"",
      ""fame"",
      ""fade"",
      ""fan""
    ]
  },
  {
    ""word"": ""faith"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""face"",
      ""fame"",
      ""father"",
      ""fat"",
      ""fade""
    ]
  },
  {
    ""word"": ""fall"",
    ""rhymes"": [
      ""all"",
      ""ball"",
      ""call"",
      ""wall"",
      ""recall"",
      ""small"",
      ""alcohol"",
      ""doll"",
      ""install"",
      ""crawl""
    ],
    ""soundsLike"": [
      ""feel"",
      ""file"",
      ""foil"",
      ""fault"",
      ""false""
    ]
  },
  {
    ""word"": ""false"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""fall"",
      ""follow"",
      ""face""
    ]
  },
  {
    ""word"": ""fame"",
    ""rhymes"": [
      ""game"",
      ""name"",
      ""frame"",
      ""claim"",
      ""aim"",
      ""same"",
      ""blame"",
      ""flame""
    ],
    ""soundsLike"": [
      ""foam"",
      ""fun"",
      ""fine"",
      ""phone"",
      ""faith"",
      ""flame"",
      ""fan"",
      ""frame""
    ]
  },
  {
    ""word"": ""family"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""final"",
      ""female""
    ]
  },
  {
    ""word"": ""famous"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""face"",
      ""fame"",
      ""fantasy""
    ]
  },
  {
    ""word"": ""fan"",
    ""rhymes"": [
      ""man"",
      ""can"",
      ""scan"",
      ""van""
    ],
    ""soundsLike"": [
      ""fun"",
      ""fine"",
      ""phone"",
      ""fame"",
      ""foam""
    ]
  },
  {
    ""word"": ""fancy"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""fantasy"",
      ""funny"",
      ""fence""
    ]
  },
  {
    ""word"": ""fantasy"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""fancy""
    ]
  },
  {
    ""word"": ""farm"",
    ""rhymes"": [
      ""arm"",
      ""alarm""
    ],
    ""soundsLike"": [
      ""firm"",
      ""forum"",
      ""fame""
    ]
  },
  {
    ""word"": ""fashion"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""fiction""
    ]
  },
  {
    ""word"": ""fat"",
    ""rhymes"": [
      ""cat"",
      ""that"",
      ""hat"",
      ""flat"",
      ""chat""
    ],
    ""soundsLike"": [
      ""fit"",
      ""foot"",
      ""food"",
      ""fade"",
      ""flat"",
      ""feed""
    ]
  },
  {
    ""word"": ""fatal"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""vital"",
      ""fuel""
    ]
  },
  {
    ""word"": ""father"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""faith"",
      ""feature""
    ]
  },
  {
    ""word"": ""fatigue"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""photo""
    ]
  },
  {
    ""word"": ""fault"",
    ""rhymes"": [
      ""salt"",
      ""vault"",
      ""assault""
    ],
    ""soundsLike"": [
      ""fall"",
      ""field"",
      ""fold"",
      ""vault"",
      ""follow""
    ]
  },
  {
    ""word"": ""favorite"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""forest"",
      ""fever""
    ]
  },
  {
    ""word"": ""feature"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""fever"",
      ""father"",
      ""fetch""
    ]
  },
  {
    ""word"": ""february"",
    ""rhymes"": [
      ""cherry"",
      ""carry"",
      ""library"",
      ""primary"",
      ""very"",
      ""ordinary"",
      ""merry""
    ],
    ""soundsLike"": []
  },
  {
    ""word"": ""federal"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""fatal""
    ]
  },
  {
    ""word"": ""fee"",
    ""rhymes"": [
      ""tree"",
      ""sea"",
      ""key"",
      ""flee"",
      ""degree"",
      ""debris"",
      ""agree"",
      ""ski"",
      ""three"",
      ""pony"",
      ""knee""
    ],
    ""soundsLike"": [
      ""feed""
    ]
  },
  {
    ""word"": ""feed"",
    ""rhymes"": [
      ""speed"",
      ""seed"",
      ""need""
    ],
    ""soundsLike"": [
      ""food"",
      ""fade"",
      ""field"",
      ""fit"",
      ""fat"",
      ""foot"",
      ""fee""
    ]
  },
  {
    ""word"": ""feel"",
    ""rhymes"": [
      ""deal"",
      ""wheel"",
      ""steel"",
      ""real"",
      ""reveal""
    ],
    ""soundsLike"": [
      ""fall"",
      ""file"",
      ""foil"",
      ""field"",
      ""fee""
    ]
  },
  {
    ""word"": ""female"",
    ""rhymes"": [
      ""scale"",
      ""rail"",
      ""mail"",
      ""whale"",
      ""detail"",
      ""tail"",
      ""sail"",
      ""unveil"",
      ""inhale""
    ],
    ""soundsLike"": [
      ""final"",
      ""family"",
      ""feel""
    ]
  },
  {
    ""word"": ""fence"",
    ""rhymes"": [
      ""sense"",
      ""defense"",
      ""immense""
    ],
    ""soundsLike"": [
      ""fun"",
      ""funny"",
      ""face"",
      ""fan"",
      ""fancy""
    ]
  },
  {
    ""word"": ""festival"",
    ""rhymes"": [],
    ""soundsLike"": []
  },
  {
    ""word"": ""fetch"",
    ""rhymes"": [
      ""sketch""
    ],
    ""soundsLike"": [
      ""fish"",
      ""fit""
    ]
  },
  {
    ""word"": ""fever"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""feature"",
      ""fire"",
      ""flavor""
    ]
  },
  {
    ""word"": ""few"",
    ""rhymes"": [
      ""blue"",
      ""into"",
      ""you"",
      ""view"",
      ""review"",
      ""true"",
      ""two"",
      ""shoe"",
      ""crew"",
      ""tissue"",
      ""glue"",
      ""bamboo"",
      ""undo"",
      ""tattoo"",
      ""renew"",
      ""canoe"",
      ""zoo""
    ],
    ""soundsLike"": [
      ""fuel"",
      ""view""
    ]
  },
  {
    ""word"": ""fiber"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""fire""
    ]
  },
  {
    ""word"": ""fiction"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""fashion"",
      ""section"",
      ""auction""
    ]
  },
  {
    ""word"": ""field"",
    ""rhymes"": [
      ""shield""
    ],
    ""soundsLike"": [
      ""fold"",
      ""feel"",
      ""fault"",
      ""feed"",
      ""fall"",
      ""fade""
    ]
  },
  {
    ""word"": ""figure"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""finger""
    ]
  },
  {
    ""word"": ""file"",
    ""rhymes"": [
      ""style"",
      ""trial"",
      ""aisle"",
      ""smile"",
      ""exile"",
      ""dial""
    ],
    ""soundsLike"": [
      ""feel"",
      ""fall"",
      ""foil""
    ]
  },
  {
    ""word"": ""film"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""feel"",
      ""fall"",
      ""fame""
    ]
  },
  {
    ""word"": ""filter"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""fault""
    ]
  },
  {
    ""word"": ""final"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""female"",
      ""finish"",
      ""family""
    ]
  },
  {
    ""word"": ""find"",
    ""rhymes"": [
      ""bind"",
      ""mind"",
      ""kind"",
      ""blind"",
      ""behind"",
      ""remind""
    ],
    ""soundsLike"": [
      ""found"",
      ""fine"",
      ""faint"",
      ""fade"",
      ""fan"",
      ""feed""
    ]
  },
  {
    ""word"": ""fine"",
    ""rhymes"": [
      ""design"",
      ""sign"",
      ""wine"",
      ""genuine"",
      ""shine"",
      ""decline"",
      ""define"",
      ""online"",
      ""combine""
    ],
    ""soundsLike"": [
      ""fun"",
      ""phone"",
      ""fan"",
      ""find"",
      ""fame"",
      ""foam""
    ]
  },
  {
    ""word"": ""finger"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""figure""
    ]
  },
  {
    ""word"": ""finish"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""vanish"",
      ""funny"",
      ""final""
    ]
  },
  {
    ""word"": ""fire"",
    ""rhymes"": [
      ""wire"",
      ""inspire"",
      ""acquire"",
      ""require"",
      ""hire"",
      ""entire"",
      ""liar"",
      ""retire"",
      ""expire"",
      ""buyer""
    ],
    ""soundsLike"": [
      ""fiber""
    ]
  },
  {
    ""word"": ""firm"",
    ""rhymes"": [
      ""term"",
      ""confirm""
    ],
    ""soundsLike"": [
      ""farm"",
      ""frame"",
      ""arm""
    ]
  },
  {
    ""word"": ""first"",
    ""rhymes"": [
      ""burst""
    ],
    ""soundsLike"": [
      ""fruit"",
      ""frost""
    ]
  },
  {
    ""word"": ""fiscal"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""physical"",
      ""fossil""
    ]
  },
  {
    ""word"": ""fish"",
    ""rhymes"": [
      ""dish"",
      ""wish""
    ],
    ""soundsLike"": [
      ""fetch"",
      ""fit"",
      ""flush""
    ]
  },
  {
    ""word"": ""fit"",
    ""rhymes"": [
      ""split"",
      ""grit"",
      ""quit"",
      ""kit"",
      ""submit"",
      ""admit"",
      ""permit"",
      ""omit""
    ],
    ""soundsLike"": [
      ""fat"",
      ""foot"",
      ""fish"",
      ""fade"",
      ""feed""
    ]
  },
  {
    ""word"": ""fitness"",
    ""rhymes"": [
      ""witness""
    ],
    ""soundsLike"": [
      ""witness"",
      ""famous"",
      ""furnace"",
      ""fence""
    ]
  },
  {
    ""word"": ""fix"",
    ""rhymes"": [
      ""mix"",
      ""six""
    ],
    ""soundsLike"": [
      ""fox"",
      ""face""
    ]
  },
  {
    ""word"": ""flag"",
    ""rhymes"": [
      ""bag"",
      ""tag""
    ],
    ""soundsLike"": [
      ""flock"",
      ""flash"",
      ""flat"",
      ""flee""
    ]
  },
  {
    ""word"": ""flame"",
    ""rhymes"": [
      ""game"",
      ""name"",
      ""frame"",
      ""claim"",
      ""aim"",
      ""same"",
      ""blame"",
      ""fame""
    ],
    ""soundsLike"": [
      ""fame"",
      ""flee""
    ]
  },
  {
    ""word"": ""flash"",
    ""rhymes"": [
      ""dash"",
      ""crash"",
      ""trash"",
      ""cash""
    ],
    ""soundsLike"": [
      ""flush"",
      ""flat"",
      ""fish"",
      ""flag"",
      ""flee""
    ]
  },
  {
    ""word"": ""flat"",
    ""rhymes"": [
      ""cat"",
      ""that"",
      ""hat"",
      ""fat"",
      ""chat""
    ],
    ""soundsLike"": [
      ""flight"",
      ""float"",
      ""fat"",
      ""flash""
    ]
  },
  {
    ""word"": ""flavor"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""floor"",
      ""flower"",
      ""fever""
    ]
  },
  {
    ""word"": ""flee"",
    ""rhymes"": [
      ""tree"",
      ""sea"",
      ""key"",
      ""degree"",
      ""debris"",
      ""agree"",
      ""fee"",
      ""ski"",
      ""three"",
      ""pony"",
      ""knee""
    ],
    ""soundsLike"": [
      ""fly"",
      ""fee""
    ]
  },
  {
    ""word"": ""flight"",
    ""rhymes"": [
      ""light"",
      ""right"",
      ""night"",
      ""write"",
      ""sight"",
      ""bright"",
      ""slight"",
      ""height"",
      ""kite"",
      ""invite"",
      ""excite"",
      ""midnight""
    ],
    ""soundsLike"": [
      ""flat"",
      ""float"",
      ""fly"",
      ""fat""
    ]
  },
  {
    ""word"": ""flip"",
    ""rhymes"": [
      ""ship"",
      ""trip"",
      ""tip"",
      ""clip"",
      ""whip"",
      ""drip"",
      ""hip"",
      ""equip""
    ],
    ""soundsLike"": [
      ""fly"",
      ""flee""
    ]
  },
  {
    ""word"": ""float"",
    ""rhymes"": [
      ""note"",
      ""boat"",
      ""promote"",
      ""quote"",
      ""goat"",
      ""vote"",
      ""devote""
    ],
    ""soundsLike"": [
      ""flight"",
      ""flat"",
      ""fit"",
      ""fat""
    ]
  },
  {
    ""word"": ""flock"",
    ""rhymes"": [
      ""stock"",
      ""talk"",
      ""lock"",
      ""clock"",
      ""walk"",
      ""shock"",
      ""hawk"",
      ""knock"",
      ""chalk"",
      ""sock"",
      ""unlock""
    ],
    ""soundsLike"": [
      ""flag""
    ]
  },
  {
    ""word"": ""floor"",
    ""rhymes"": [
      ""door"",
      ""more"",
      ""core"",
      ""before"",
      ""ignore"",
      ""dinosaur"",
      ""outdoor"",
      ""indoor""
    ],
    ""soundsLike"": [
      ""flower"",
      ""fire""
    ]
  },
  {
    ""word"": ""flower"",
    ""rhymes"": [
      ""power"",
      ""hour"",
      ""empower"",
      ""tower""
    ],
    ""soundsLike"": [
      ""floor"",
      ""fire""
    ]
  },
  {
    ""word"": ""fluid"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""flight"",
      ""flat"",
      ""float""
    ]
  },
  {
    ""word"": ""flush"",
    ""rhymes"": [
      ""crush"",
      ""brush"",
      ""blush"",
      ""slush""
    ],
    ""soundsLike"": [
      ""flash"",
      ""fish"",
      ""floor"",
      ""flight"",
      ""flat"",
      ""flee"",
      ""float""
    ]
  },
  {
    ""word"": ""fly"",
    ""rhymes"": [
      ""eye"",
      ""high"",
      ""supply"",
      ""identify"",
      ""dry"",
      ""shy"",
      ""cry"",
      ""clarify"",
      ""try"",
      ""verify"",
      ""deny"",
      ""defy"",
      ""rely"",
      ""modify"",
      ""satisfy"",
      ""spy""
    ],
    ""soundsLike"": [
      ""flee"",
      ""flight"",
      ""fee""
    ]
  },
  {
    ""word"": ""foam"",
    ""rhymes"": [
      ""home""
    ],
    ""soundsLike"": [
      ""phone"",
      ""fame"",
      ""fun"",
      ""fine"",
      ""fan""
    ]
  },
  {
    ""word"": ""focus"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""fix"",
      ""fox""
    ]
  },
  {
    ""word"": ""fog"",
    ""rhymes"": [
      ""dog"",
      ""frog"",
      ""hedgehog"",
      ""clog""
    ],
    ""soundsLike"": [
      ""frog""
    ]
  },
  {
    ""word"": ""foil"",
    ""rhymes"": [
      ""oil"",
      ""spoil"",
      ""coil"",
      ""boil""
    ],
    ""soundsLike"": [
      ""feel"",
      ""fall"",
      ""file"",
      ""awful""
    ]
  },
  {
    ""word"": ""fold"",
    ""rhymes"": [
      ""hold"",
      ""gold"",
      ""old"",
      ""uphold"",
      ""unfold""
    ],
    ""soundsLike"": [
      ""field"",
      ""fault"",
      ""feel"",
      ""fall"",
      ""fade"",
      ""feed""
    ]
  },
  {
    ""word"": ""follow"",
    ""rhymes"": [
      ""hollow"",
      ""swallow""
    ],
    ""soundsLike"": [
      ""fall"",
      ""fault"",
      ""false""
    ]
  },
  {
    ""word"": ""food"",
    ""rhymes"": [
      ""attitude"",
      ""rude"",
      ""include"",
      ""exclude""
    ],
    ""soundsLike"": [
      ""fade"",
      ""feed"",
      ""fit"",
      ""fat"",
      ""foot""
    ]
  },
  {
    ""word"": ""foot"",
    ""rhymes"": [
      ""put"",
      ""input"",
      ""output""
    ],
    ""soundsLike"": [
      ""fit"",
      ""fat"",
      ""food"",
      ""fade"",
      ""feed""
    ]
  },
  {
    ""word"": ""force"",
    ""rhymes"": [
      ""course"",
      ""horse"",
      ""source"",
      ""endorse"",
      ""enforce"",
      ""divorce""
    ],
    ""soundsLike"": [
      ""face""
    ]
  },
  {
    ""word"": ""forest"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""frost"",
      ""first"",
      ""force""
    ]
  },
  {
    ""word"": ""forget"",
    ""rhymes"": [
      ""net"",
      ""upset"",
      ""asset"",
      ""wet"",
      ""regret"",
      ""pet"",
      ""sunset""
    ],
    ""soundsLike"": [
      ""fruit"",
      ""first""
    ]
  },
  {
    ""word"": ""fork"",
    ""rhymes"": [],
    ""soundsLike"": []
  },
  {
    ""word"": ""fortune"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""forum"",
      ""fashion""
    ]
  },
  {
    ""word"": ""forum"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""farm"",
      ""frame"",
      ""fortune""
    ]
  },
  {
    ""word"": ""forward"",
    ""rhymes"": [],
    ""soundsLike"": []
  },
  {
    ""word"": ""fossil"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""fiscal"",
      ""vessel""
    ]
  },
  {
    ""word"": ""foster"",
    ""rhymes"": [],
    ""soundsLike"": []
  },
  {
    ""word"": ""found"",
    ""rhymes"": [
      ""around"",
      ""round"",
      ""sound"",
      ""surround""
    ],
    ""soundsLike"": [
      ""find"",
      ""faint"",
      ""fade"",
      ""fan""
    ]
  },
  {
    ""word"": ""fox"",
    ""rhymes"": [
      ""box""
    ],
    ""soundsLike"": [
      ""fix"",
      ""face""
    ]
  },
  {
    ""word"": ""fragile"",
    ""rhymes"": [],
    ""soundsLike"": []
  },
  {
    ""word"": ""frame"",
    ""rhymes"": [
      ""game"",
      ""name"",
      ""claim"",
      ""aim"",
      ""same"",
      ""blame"",
      ""flame"",
      ""fame""
    ],
    ""soundsLike"": [
      ""fame"",
      ""frown"",
      ""firm"",
      ""flame"",
      ""foam"",
      ""forum""
    ]
  },
  {
    ""word"": ""frequent"",
    ""rhymes"": [
      ""present"",
      ""rent"",
      ""prevent"",
      ""segment"",
      ""tent"",
      ""orient"",
      ""cement""
    ],
    ""soundsLike"": [
      ""front""
    ]
  },
  {
    ""word"": ""fresh"",
    ""rhymes"": [
      ""mesh""
    ],
    ""soundsLike"": [
      ""fish"",
      ""fetch""
    ]
  },
  {
    ""word"": ""friend"",
    ""rhymes"": [
      ""end"",
      ""lend"",
      ""attend"",
      ""trend"",
      ""extend"",
      ""spend"",
      ""depend"",
      ""weekend""
    ],
    ""soundsLike"": [
      ""front"",
      ""find"",
      ""found""
    ]
  },
  {
    ""word"": ""fringe"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""orange"",
      ""frown"",
      ""friend"",
      ""fresh"",
      ""front""
    ]
  },
  {
    ""word"": ""frog"",
    ""rhymes"": [
      ""dog"",
      ""fog"",
      ""hedgehog"",
      ""clog""
    ],
    ""soundsLike"": [
      ""fog""
    ]
  },
  {
    ""word"": ""front"",
    ""rhymes"": [
      ""hunt"",
      ""grunt""
    ],
    ""soundsLike"": [
      ""friend"",
      ""faint""
    ]
  },
  {
    ""word"": ""frost"",
    ""rhymes"": [
      ""cost"",
      ""exhaust""
    ],
    ""soundsLike"": [
      ""first"",
      ""forest"",
      ""fruit""
    ]
  },
  {
    ""word"": ""frown"",
    ""rhymes"": [
      ""around"",
      ""brown"",
      ""town"",
      ""gown"",
      ""clown""
    ],
    ""soundsLike"": [
      ""frame"",
      ""fan""
    ]
  },
  {
    ""word"": ""frozen"",
    ""rhymes"": [],
    ""soundsLike"": []
  },
  {
    ""word"": ""fruit"",
    ""rhymes"": [
      ""suit"",
      ""shoot"",
      ""route"",
      ""minute"",
      ""execute"",
      ""cute"",
      ""salute"",
      ""lawsuit""
    ],
    ""soundsLike"": [
      ""first"",
      ""fat""
    ]
  },
  {
    ""word"": ""fuel"",
    ""rhymes"": [
      ""school"",
      ""rule"",
      ""pool"",
      ""cool"",
      ""tool"",
      ""stool"",
      ""cruel"",
      ""mule"",
      ""unusual"",
      ""jewel""
    ],
    ""soundsLike"": [
      ""fatal"",
      ""few"",
      ""final"",
      ""fossil""
    ]
  },
  {
    ""word"": ""fun"",
    ""rhymes"": [
      ""run"",
      ""one"",
      ""gun"",
      ""sun"",
      ""someone""
    ],
    ""soundsLike"": [
      ""fine"",
      ""phone"",
      ""fan"",
      ""funny"",
      ""fame"",
      ""foam""
    ]
  },
  {
    ""word"": ""funny"",
    ""rhymes"": [
      ""honey"",
      ""sunny""
    ],
    ""soundsLike"": [
      ""fun"",
      ""fence""
    ]
  },
  {
    ""word"": ""furnace"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""famous"",
      ""fence"",
      ""fantasy""
    ]
  },
  {
    ""word"": ""fury"",
    ""rhymes"": [],
    ""soundsLike"": []
  },
  {
    ""word"": ""future"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""feature"",
      ""few"",
      ""fuel""
    ]
  },
  {
    ""word"": ""gadget"",
    ""rhymes"": [],
    ""soundsLike"": []
  },
  {
    ""word"": ""gain"",
    ""rhymes"": [
      ""train"",
      ""domain"",
      ""sustain"",
      ""rain"",
      ""again"",
      ""grain"",
      ""obtain"",
      ""brain"",
      ""explain"",
      ""main"",
      ""crane"",
      ""insane"",
      ""remain""
    ],
    ""soundsLike"": [
      ""game"",
      ""gun"",
      ""gown"",
      ""again"",
      ""grain""
    ]
  },
  {
    ""word"": ""galaxy"",
    ""rhymes"": [],
    ""soundsLike"": []
  },
  {
    ""word"": ""gallery"",
    ""rhymes"": [],
    ""soundsLike"": []
  },
  {
    ""word"": ""game"",
    ""rhymes"": [
      ""name"",
      ""frame"",
      ""claim"",
      ""aim"",
      ""same"",
      ""blame"",
      ""flame"",
      ""fame""
    ],
    ""soundsLike"": [
      ""gain"",
      ""gun"",
      ""gown"",
      ""calm""
    ]
  },
  {
    ""word"": ""gap"",
    ""rhymes"": [
      ""snap"",
      ""trap"",
      ""wrap"",
      ""scrap"",
      ""clap""
    ],
    ""soundsLike"": [
      ""gasp"",
      ""cup"",
      ""keep""
    ]
  },
  {
    ""word"": ""garage"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""grace"",
      ""grain"",
      ""crouch""
    ]
  },
  {
    ""word"": ""garbage"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""cabbage"",
      ""carbon""
    ]
  },
  {
    ""word"": ""garden"",
    ""rhymes"": [],
    ""soundsLike"": []
  },
  {
    ""word"": ""garlic"",
    ""rhymes"": [],
    ""soundsLike"": []
  },
  {
    ""word"": ""garment"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""current"",
      ""grunt""
    ]
  },
  {
    ""word"": ""gas"",
    ""rhymes"": [
      ""grass"",
      ""pass"",
      ""glass"",
      ""mass"",
      ""brass""
    ],
    ""soundsLike"": [
      ""guess"",
      ""goose"",
      ""glass"",
      ""grass"",
      ""gaze"",
      ""gasp"",
      ""case""
    ]
  },
  {
    ""word"": ""gasp"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""gas"",
      ""gap"",
      ""guess"",
      ""goose"",
      ""gossip""
    ]
  },
  {
    ""word"": ""gate"",
    ""rhymes"": [
      ""state"",
      ""rate"",
      ""plate"",
      ""great"",
      ""mandate"",
      ""estate"",
      ""indicate"",
      ""update"",
      ""wait"",
      ""debate""
    ],
    ""soundsLike"": [
      ""goat"",
      ""good"",
      ""great"",
      ""guide"",
      ""gaze""
    ]
  },
  {
    ""word"": ""gather"",
    ""rhymes"": [
      ""rather""
    ],
    ""soundsLike"": []
  },
  {
    ""word"": ""gauge"",
    ""rhymes"": [
      ""age"",
      ""engage"",
      ""stage"",
      ""page"",
      ""cage"",
      ""wage""
    ],
    ""soundsLike"": [
      ""cage"",
      ""game"",
      ""gain""
    ]
  },
  {
    ""word"": ""gaze"",
    ""rhymes"": [
      ""raise"",
      ""phrase"",
      ""praise"",
      ""always"",
      ""maze""
    ],
    ""soundsLike"": [
      ""gate"",
      ""gas"",
      ""goose""
    ]
  },
  {
    ""word"": ""general"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""gentle""
    ]
  },
  {
    ""word"": ""genius"",
    ""rhymes"": [],
    ""soundsLike"": []
  },
  {
    ""word"": ""genre"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""camera""
    ]
  },
  {
    ""word"": ""gentle"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""general""
    ]
  },
  {
    ""word"": ""genuine"",
    ""rhymes"": [
      ""design"",
      ""sign"",
      ""fine"",
      ""wine"",
      ""shine"",
      ""decline"",
      ""define"",
      ""online"",
      ""combine""
    ],
    ""soundsLike"": []
  },
  {
    ""word"": ""gesture"",
    ""rhymes"": [],
    ""soundsLike"": []
  },
  {
    ""word"": ""ghost"",
    ""rhymes"": [
      ""post"",
      ""host"",
      ""coast"",
      ""roast"",
      ""toast"",
      ""almost""
    ],
    ""soundsLike"": [
      ""coast"",
      ""goat"",
      ""cost"",
      ""gate"",
      ""gas"",
      ""august""
    ]
  },
  {
    ""word"": ""giant"",
    ""rhymes"": [
      ""client""
    ],
    ""soundsLike"": [
      ""agent""
    ]
  },
  {
    ""word"": ""gift"",
    ""rhymes"": [
      ""shift"",
      ""lift"",
      ""drift"",
      ""swift""
    ],
    ""soundsLike"": [
      ""gate"",
      ""goat"",
      ""guilt"",
      ""kit""
    ]
  },
  {
    ""word"": ""giggle"",
    ""rhymes"": [],
    ""soundsLike"": []
  },
  {
    ""word"": ""ginger"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""junior"",
      ""change""
    ]
  },
  {
    ""word"": ""giraffe"",
    ""rhymes"": [
      ""staff"",
      ""half"",
      ""laugh""
    ],
    ""soundsLike"": [
      ""grief""
    ]
  },
  {
    ""word"": ""girl"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""grow""
    ]
  },
  {
    ""word"": ""give"",
    ""rhymes"": [
      ""live""
    ],
    ""soundsLike"": [
      ""cave"",
      ""glove""
    ]
  },
  {
    ""word"": ""glad"",
    ""rhymes"": [
      ""sad"",
      ""add"",
      ""mad"",
      ""dad""
    ],
    ""soundsLike"": [
      ""glide"",
      ""cloud"",
      ""glass""
    ]
  },
  {
    ""word"": ""glance"",
    ""rhymes"": [
      ""enhance"",
      ""dance"",
      ""advance"",
      ""romance""
    ],
    ""soundsLike"": [
      ""glass""
    ]
  },
  {
    ""word"": ""glare"",
    ""rhymes"": [
      ""air"",
      ""chair"",
      ""wear"",
      ""square"",
      ""share"",
      ""despair"",
      ""there"",
      ""hair"",
      ""spare"",
      ""aware"",
      ""pair"",
      ""where"",
      ""rare"",
      ""swear"",
      ""pear"",
      ""affair"",
      ""repair"",
      ""prepare"",
      ""scare"",
      ""warfare"",
      ""unfair"",
      ""unaware""
    ],
    ""soundsLike"": []
  },
  {
    ""word"": ""glass"",
    ""rhymes"": [
      ""grass"",
      ""pass"",
      ""mass"",
      ""gas"",
      ""brass""
    ],
    ""soundsLike"": [
      ""gas"",
      ""glance"",
      ""close"",
      ""glad""
    ]
  },
  {
    ""word"": ""glide"",
    ""rhymes"": [
      ""side"",
      ""provide"",
      ""ride"",
      ""slide"",
      ""pride"",
      ""tide"",
      ""guide"",
      ""decide"",
      ""divide"",
      ""wide"",
      ""inside"",
      ""outside""
    ],
    ""soundsLike"": [
      ""glad"",
      ""guide"",
      ""cloud""
    ]
  },
  {
    ""word"": ""glimpse"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""glance"",
      ""clump""
    ]
  },
  {
    ""word"": ""globe"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""glow"",
      ""club"",
      ""glue"",
      ""clap"",
      ""clip""
    ]
  },
  {
    ""word"": ""gloom"",
    ""rhymes"": [
      ""room"",
      ""assume"",
      ""broom""
    ],
    ""soundsLike"": [
      ""glue"",
      ""climb"",
      ""claim"",
      ""game""
    ]
  },
  {
    ""word"": ""glory"",
    ""rhymes"": [
      ""story"",
      ""category""
    ],
    ""soundsLike"": [
      ""glare""
    ]
  },
  {
    ""word"": ""glove"",
    ""rhymes"": [
      ""love"",
      ""above"",
      ""dove"",
      ""shove""
    ],
    ""soundsLike"": [
      ""olive"",
      ""give"",
      ""glow"",
      ""glue"",
      ""clutch"",
      ""glare""
    ]
  },
  {
    ""word"": ""glow"",
    ""rhymes"": [
      ""know"",
      ""snow"",
      ""throw"",
      ""hello"",
      ""grow"",
      ""shadow"",
      ""potato"",
      ""toe"",
      ""slow"",
      ""tomato"",
      ""below"",
      ""radio"",
      ""meadow"",
      ""tomorrow"",
      ""borrow"",
      ""photo"",
      ""tornado"",
      ""buffalo""
    ],
    ""soundsLike"": [
      ""glue"",
      ""globe"",
      ""clay"",
      ""claw""
    ]
  },
  {
    ""word"": ""glue"",
    ""rhymes"": [
      ""blue"",
      ""into"",
      ""you"",
      ""view"",
      ""review"",
      ""true"",
      ""two"",
      ""shoe"",
      ""crew"",
      ""tissue"",
      ""few"",
      ""bamboo"",
      ""undo"",
      ""tattoo"",
      ""renew"",
      ""canoe"",
      ""zoo""
    ],
    ""soundsLike"": [
      ""glow"",
      ""gloom"",
      ""clay"",
      ""claw""
    ]
  },
  {
    ""word"": ""goat"",
    ""rhymes"": [
      ""note"",
      ""boat"",
      ""promote"",
      ""float"",
      ""quote"",
      ""vote"",
      ""devote""
    ],
    ""soundsLike"": [
      ""gate"",
      ""good"",
      ""ghost""
    ]
  },
  {
    ""word"": ""goddess"",
    ""rhymes"": [],
    ""soundsLike"": []
  },
  {
    ""word"": ""gold"",
    ""rhymes"": [
      ""hold"",
      ""fold"",
      ""old"",
      ""uphold"",
      ""unfold""
    ],
    ""soundsLike"": [
      ""guilt""
    ]
  },
  {
    ""word"": ""good"",
    ""rhymes"": [
      ""bid"",
      ""wood"",
      ""grid"",
      ""kid"",
      ""hood""
    ],
    ""soundsLike"": [
      ""guide"",
      ""gate"",
      ""goat"",
      ""code""
    ]
  },
  {
    ""word"": ""goose"",
    ""rhymes"": [
      ""use"",
      ""abuse"",
      ""produce"",
      ""juice"",
      ""reduce"",
      ""excuse""
    ],
    ""soundsLike"": [
      ""gas"",
      ""guess"",
      ""gaze"",
      ""juice"",
      ""case""
    ]
  },
  {
    ""word"": ""gorilla"",
    ""rhymes"": [],
    ""soundsLike"": []
  },
  {
    ""word"": ""gospel"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""castle"",
      ""couple""
    ]
  },
  {
    ""word"": ""gossip"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""gasp""
    ]
  },
  {
    ""word"": ""govern"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""cover"",
      ""gun"",
      ""corn""
    ]
  },
  {
    ""word"": ""gown"",
    ""rhymes"": [
      ""around"",
      ""brown"",
      ""town"",
      ""frown"",
      ""clown""
    ],
    ""soundsLike"": [
      ""gun"",
      ""gain"",
      ""game"",
      ""can""
    ]
  },
  {
    ""word"": ""grab"",
    ""rhymes"": [
      ""lab"",
      ""slab""
    ],
    ""soundsLike"": [
      ""group"",
      ""grape"",
      ""grow"",
      ""gap""
    ]
  },
  {
    ""word"": ""grace"",
    ""rhymes"": [
      ""case"",
      ""face"",
      ""base"",
      ""place"",
      ""space"",
      ""race"",
      ""embrace"",
      ""chase"",
      ""replace"",
      ""erase""
    ],
    ""soundsLike"": [
      ""grass"",
      ""cross"",
      ""erase""
    ]
  },
  {
    ""word"": ""grain"",
    ""rhymes"": [
      ""train"",
      ""domain"",
      ""sustain"",
      ""rain"",
      ""again"",
      ""obtain"",
      ""brain"",
      ""gain"",
      ""explain"",
      ""main"",
      ""crane"",
      ""insane"",
      ""remain""
    ],
    ""soundsLike"": [
      ""green"",
      ""crane"",
      ""gain""
    ]
  },
  {
    ""word"": ""grant"",
    ""rhymes"": [
      ""aunt""
    ],
    ""soundsLike"": [
      ""grunt"",
      ""current"",
      ""great"",
      ""green"",
      ""grain""
    ]
  },
  {
    ""word"": ""grape"",
    ""rhymes"": [
      ""tape"",
      ""escape""
    ],
    ""soundsLike"": [
      ""group"",
      ""grab"",
      ""crop"",
      ""grace"",
      ""gap""
    ]
  },
  {
    ""word"": ""grass"",
    ""rhymes"": [
      ""pass"",
      ""glass"",
      ""mass"",
      ""gas"",
      ""brass""
    ],
    ""soundsLike"": [
      ""grace"",
      ""gas"",
      ""cross""
    ]
  },
  {
    ""word"": ""gravity"",
    ""rhymes"": [],
    ""soundsLike"": []
  },
  {
    ""word"": ""great"",
    ""rhymes"": [
      ""state"",
      ""rate"",
      ""plate"",
      ""mandate"",
      ""estate"",
      ""gate"",
      ""indicate"",
      ""update"",
      ""wait"",
      ""debate""
    ],
    ""soundsLike"": [
      ""grit"",
      ""gate"",
      ""grid""
    ]
  },
  {
    ""word"": ""green"",
    ""rhymes"": [
      ""mean"",
      ""bean"",
      ""machine"",
      ""keen"",
      ""screen"",
      ""clean"",
      ""between"",
      ""scene"",
      ""marine""
    ],
    ""soundsLike"": [
      ""grain"",
      ""cream"",
      ""crane""
    ]
  },
  {
    ""word"": ""grid"",
    ""rhymes"": [
      ""bid"",
      ""good"",
      ""kid""
    ],
    ""soundsLike"": [
      ""grit"",
      ""great"",
      ""crowd""
    ]
  },
  {
    ""word"": ""grief"",
    ""rhymes"": [
      ""leaf"",
      ""relief"",
      ""brief"",
      ""chief"",
      ""beef""
    ],
    ""soundsLike"": []
  },
  {
    ""word"": ""grit"",
    ""rhymes"": [
      ""fit"",
      ""split"",
      ""quit"",
      ""kit"",
      ""submit"",
      ""admit"",
      ""permit"",
      ""omit""
    ],
    ""soundsLike"": [
      ""great"",
      ""grid""
    ]
  },
  {
    ""word"": ""grocery"",
    ""rhymes"": [],
    ""soundsLike"": []
  },
  {
    ""word"": ""group"",
    ""rhymes"": [
      ""loop"",
      ""soup""
    ],
    ""soundsLike"": [
      ""grape"",
      ""grab"",
      ""crop"",
      ""gap""
    ]
  },
  {
    ""word"": ""grow"",
    ""rhymes"": [
      ""know"",
      ""snow"",
      ""throw"",
      ""hello"",
      ""shadow"",
      ""potato"",
      ""toe"",
      ""glow"",
      ""slow"",
      ""tomato"",
      ""below"",
      ""radio"",
      ""meadow"",
      ""tomorrow"",
      ""borrow"",
      ""photo"",
      ""tornado"",
      ""buffalo""
    ],
    ""soundsLike"": [
      ""girl""
    ]
  },
  {
    ""word"": ""grunt"",
    ""rhymes"": [
      ""front"",
      ""hunt""
    ],
    ""soundsLike"": [
      ""grant"",
      ""current"",
      ""grit"",
      ""grain""
    ]
  },
  {
    ""word"": ""guard"",
    ""rhymes"": [
      ""card"",
      ""hard"",
      ""yard""
    ],
    ""soundsLike"": [
      ""card"",
      ""yard"",
      ""cart""
    ]
  },
  {
    ""word"": ""guess"",
    ""rhymes"": [
      ""process"",
      ""address"",
      ""access"",
      ""express"",
      ""success"",
      ""dress"",
      ""excess"",
      ""bless""
    ],
    ""soundsLike"": [
      ""gas"",
      ""goose"",
      ""kiss"",
      ""gaze"",
      ""case""
    ]
  },
  {
    ""word"": ""guide"",
    ""rhymes"": [
      ""side"",
      ""provide"",
      ""ride"",
      ""slide"",
      ""pride"",
      ""tide"",
      ""decide"",
      ""divide"",
      ""wide"",
      ""glide"",
      ""inside"",
      ""outside""
    ],
    ""soundsLike"": [
      ""good"",
      ""gate"",
      ""goat"",
      ""glide"",
      ""code""
    ]
  },
  {
    ""word"": ""guilt"",
    ""rhymes"": [
      ""tilt""
    ],
    ""soundsLike"": [
      ""gold""
    ]
  },
  {
    ""word"": ""guitar"",
    ""rhymes"": [
      ""bar"",
      ""car"",
      ""jar"",
      ""radar"",
      ""seminar"",
      ""jaguar"",
      ""cigar""
    ],
    ""soundsLike"": []
  },
  {
    ""word"": ""gun"",
    ""rhymes"": [
      ""run"",
      ""one"",
      ""fun"",
      ""sun"",
      ""someone""
    ],
    ""soundsLike"": [
      ""gain"",
      ""gown"",
      ""can"",
      ""game""
    ]
  },
  {
    ""word"": ""gym"",
    ""rhymes"": [
      ""swim"",
      ""trim"",
      ""limb"",
      ""slim""
    ],
    ""soundsLike"": [
      ""join""
    ]
  },
  {
    ""word"": ""habit"",
    ""rhymes"": [
      ""rabbit""
    ],
    ""soundsLike"": [
      ""rabbit"",
      ""robot""
    ]
  },
  {
    ""word"": ""hair"",
    ""rhymes"": [
      ""air"",
      ""chair"",
      ""wear"",
      ""square"",
      ""share"",
      ""despair"",
      ""there"",
      ""spare"",
      ""aware"",
      ""pair"",
      ""where"",
      ""rare"",
      ""swear"",
      ""pear"",
      ""affair"",
      ""repair"",
      ""prepare"",
      ""glare"",
      ""scare"",
      ""warfare"",
      ""unfair"",
      ""unaware""
    ],
    ""soundsLike"": [
      ""hire"",
      ""where"",
      ""wear""
    ]
  },
  {
    ""word"": ""half"",
    ""rhymes"": [
      ""staff"",
      ""laugh"",
      ""giraffe""
    ],
    ""soundsLike"": [
      ""have"",
      ""laugh"",
      ""wife""
    ]
  },
  {
    ""word"": ""hammer"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""hair""
    ]
  },
  {
    ""word"": ""hamster"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""hammer"",
      ""monster"",
      ""master""
    ]
  },
  {
    ""word"": ""hand"",
    ""rhymes"": [
      ""stand"",
      ""demand"",
      ""brand"",
      ""expand"",
      ""sand""
    ],
    ""soundsLike"": [
      ""hunt"",
      ""hint"",
      ""sand""
    ]
  },
  {
    ""word"": ""happy"",
    ""rhymes"": [
      ""unhappy""
    ],
    ""soundsLike"": [
      ""hobby""
    ]
  },
  {
    ""word"": ""harbor"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""horror"",
      ""harsh""
    ]
  },
  {
    ""word"": ""hard"",
    ""rhymes"": [
      ""card"",
      ""guard"",
      ""yard""
    ],
    ""soundsLike"": [
      ""heart"",
      ""yard"",
      ""card""
    ]
  },
  {
    ""word"": ""harsh"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""heart"",
      ""horse"",
      ""horror"",
      ""hard"",
      ""hair""
    ]
  },
  {
    ""word"": ""harvest"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""tourist""
    ]
  },
  {
    ""word"": ""hat"",
    ""rhymes"": [
      ""cat"",
      ""that"",
      ""flat"",
      ""fat"",
      ""chat""
    ],
    ""soundsLike"": [
      ""height"",
      ""head""
    ]
  },
  {
    ""word"": ""have"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""half"",
      ""wave""
    ]
  },
  {
    ""word"": ""hawk"",
    ""rhymes"": [
      ""stock"",
      ""talk"",
      ""lock"",
      ""clock"",
      ""walk"",
      ""shock"",
      ""flock"",
      ""knock"",
      ""chalk"",
      ""sock"",
      ""unlock""
    ],
    ""soundsLike"": [
      ""walk""
    ]
  },
  {
    ""word"": ""hazard"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""hard"",
      ""lizard"",
      ""heart""
    ]
  },
  {
    ""word"": ""head"",
    ""rhymes"": [
      ""spread"",
      ""shed"",
      ""bread"",
      ""ahead""
    ],
    ""soundsLike"": [
      ""hood"",
      ""hat"",
      ""ahead"",
      ""height""
    ]
  },
  {
    ""word"": ""health"",
    ""rhymes"": [
      ""wealth""
    ],
    ""soundsLike"": [
      ""wealth"",
      ""hello"",
      ""hill""
    ]
  },
  {
    ""word"": ""heart"",
    ""rhymes"": [
      ""art"",
      ""smart"",
      ""start"",
      ""apart"",
      ""cart"",
      ""depart""
    ],
    ""soundsLike"": [
      ""hard"",
      ""hurt"",
      ""harsh"",
      ""cart""
    ]
  },
  {
    ""word"": ""heavy"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""hover"",
      ""have""
    ]
  },
  {
    ""word"": ""hedgehog"",
    ""rhymes"": [
      ""dog"",
      ""frog"",
      ""fog"",
      ""clog""
    ],
    ""soundsLike"": []
  },
  {
    ""word"": ""height"",
    ""rhymes"": [
      ""light"",
      ""right"",
      ""night"",
      ""write"",
      ""sight"",
      ""flight"",
      ""bright"",
      ""slight"",
      ""kite"",
      ""invite"",
      ""excite"",
      ""midnight""
    ],
    ""soundsLike"": [
      ""hat"",
      ""head""
    ]
  },
  {
    ""word"": ""hello"",
    ""rhymes"": [
      ""know"",
      ""snow"",
      ""throw"",
      ""grow"",
      ""shadow"",
      ""potato"",
      ""toe"",
      ""glow"",
      ""slow"",
      ""tomato"",
      ""below"",
      ""radio"",
      ""meadow"",
      ""tomorrow"",
      ""borrow"",
      ""photo"",
      ""tornado"",
      ""buffalo""
    ],
    ""soundsLike"": [
      ""hollow"",
      ""health""
    ]
  },
  {
    ""word"": ""helmet"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""walnut""
    ]
  },
  {
    ""word"": ""help"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""hello"",
      ""hill"",
      ""hip""
    ]
  },
  {
    ""word"": ""hen"",
    ""rhymes"": [
      ""pen"",
      ""then"",
      ""again"",
      ""when"",
      ""ten""
    ],
    ""soundsLike"": [
      ""when""
    ]
  },
  {
    ""word"": ""hero"",
    ""rhymes"": [
      ""zero""
    ],
    ""soundsLike"": [
      ""zero"",
      ""hair""
    ]
  },
  {
    ""word"": ""hidden"",
    ""rhymes"": [],
    ""soundsLike"": []
  },
  {
    ""word"": ""high"",
    ""rhymes"": [
      ""fly"",
      ""eye"",
      ""supply"",
      ""identify"",
      ""dry"",
      ""shy"",
      ""cry"",
      ""clarify"",
      ""try"",
      ""verify"",
      ""deny"",
      ""defy"",
      ""rely"",
      ""modify"",
      ""satisfy"",
      ""spy""
    ],
    ""soundsLike"": []
  },
  {
    ""word"": ""hill"",
    ""rhymes"": [
      ""will"",
      ""still"",
      ""drill"",
      ""skill"",
      ""pill"",
      ""ill"",
      ""until""
    ],
    ""soundsLike"": [
      ""hole"",
      ""will""
    ]
  },
  {
    ""word"": ""hint"",
    ""rhymes"": [
      ""print""
    ],
    ""soundsLike"": [
      ""hunt"",
      ""hand"",
      ""honey"",
      ""hen""
    ]
  },
  {
    ""word"": ""hip"",
    ""rhymes"": [
      ""ship"",
      ""trip"",
      ""tip"",
      ""clip"",
      ""flip"",
      ""whip"",
      ""drip"",
      ""equip""
    ],
    ""soundsLike"": [
      ""hope"",
      ""hub"",
      ""whip"",
      ""tip""
    ]
  },
  {
    ""word"": ""hire"",
    ""rhymes"": [
      ""fire"",
      ""wire"",
      ""inspire"",
      ""acquire"",
      ""require"",
      ""entire"",
      ""liar"",
      ""retire"",
      ""expire"",
      ""buyer""
    ],
    ""soundsLike"": [
      ""hair"",
      ""wire"",
      ""high""
    ]
  },
  {
    ""word"": ""history"",
    ""rhymes"": [
      ""mystery""
    ],
    ""soundsLike"": [
      ""mystery"",
      ""destroy""
    ]
  },
  {
    ""word"": ""hobby"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""happy""
    ]
  },
  {
    ""word"": ""hockey"",
    ""rhymes"": [],
    ""soundsLike"": []
  },
  {
    ""word"": ""hold"",
    ""rhymes"": [
      ""gold"",
      ""fold"",
      ""old"",
      ""uphold"",
      ""unfold""
    ],
    ""soundsLike"": [
      ""hole""
    ]
  },
  {
    ""word"": ""hole"",
    ""rhymes"": [
      ""control"",
      ""pole"",
      ""soul"",
      ""enroll"",
      ""patrol""
    ],
    ""soundsLike"": [
      ""hill"",
      ""hold""
    ]
  },
  {
    ""word"": ""holiday"",
    ""rhymes"": [
      ""day"",
      ""away"",
      ""way"",
      ""play"",
      ""say"",
      ""survey"",
      ""display"",
      ""stay"",
      ""delay"",
      ""essay"",
      ""clay"",
      ""betray"",
      ""spray"",
      ""they"",
      ""okay"",
      ""tray"",
      ""obey"",
      ""today""
    ],
    ""soundsLike"": [
      ""melody"",
      ""solid""
    ]
  },
  {
    ""word"": ""hollow"",
    ""rhymes"": [
      ""follow"",
      ""swallow""
    ],
    ""soundsLike"": [
      ""hello""
    ]
  },
  {
    ""word"": ""home"",
    ""rhymes"": [
      ""foam""
    ],
    ""soundsLike"": [
      ""hen""
    ]
  },
  {
    ""word"": ""honey"",
    ""rhymes"": [
      ""funny"",
      ""sunny""
    ],
    ""soundsLike"": [
      ""hunt""
    ]
  },
  {
    ""word"": ""hood"",
    ""rhymes"": [
      ""good"",
      ""wood""
    ],
    ""soundsLike"": [
      ""head"",
      ""wood"",
      ""hat"",
      ""height""
    ]
  },
  {
    ""word"": ""hope"",
    ""rhymes"": [
      ""envelope"",
      ""soap""
    ],
    ""soundsLike"": [
      ""hip"",
      ""hub"",
      ""soap""
    ]
  },
  {
    ""word"": ""horn"",
    ""rhymes"": [
      ""corn""
    ],
    ""soundsLike"": [
      ""corn""
    ]
  },
  {
    ""word"": ""horror"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""harsh"",
      ""harbor"",
      ""hair""
    ]
  },
  {
    ""word"": ""horse"",
    ""rhymes"": [
      ""force"",
      ""course"",
      ""source"",
      ""endorse"",
      ""enforce"",
      ""divorce""
    ],
    ""soundsLike"": [
      ""course"",
      ""source"",
      ""harsh""
    ]
  },
  {
    ""word"": ""hospital"",
    ""rhymes"": [
      ""little""
    ],
    ""soundsLike"": [
      ""capital""
    ]
  },
  {
    ""word"": ""host"",
    ""rhymes"": [
      ""post"",
      ""coast"",
      ""roast"",
      ""ghost"",
      ""toast"",
      ""almost""
    ],
    ""soundsLike"": [
      ""toast"",
      ""coast"",
      ""post"",
      ""roast"",
      ""waste"",
      ""west"",
      ""hat""
    ]
  },
  {
    ""word"": ""hotel"",
    ""rhymes"": [
      ""spell"",
      ""shell"",
      ""tell"",
      ""sell"",
      ""rebel""
    ],
    ""soundsLike"": [
      ""total""
    ]
  },
  {
    ""word"": ""hour"",
    ""rhymes"": [
      ""power"",
      ""flower"",
      ""empower"",
      ""tower""
    ],
    ""soundsLike"": [
      ""air""
    ]
  },
  {
    ""word"": ""hover"",
    ""rhymes"": [
      ""cover"",
      ""discover"",
      ""uncover""
    ],
    ""soundsLike"": [
      ""cover"",
      ""hair"",
      ""heavy"",
      ""have"",
      ""hire""
    ]
  },
  {
    ""word"": ""hub"",
    ""rhymes"": [
      ""club"",
      ""scrub""
    ],
    ""soundsLike"": [
      ""hip"",
      ""hope"",
      ""web""
    ]
  },
  {
    ""word"": ""huge"",
    ""rhymes"": [],
    ""soundsLike"": []
  },
  {
    ""word"": ""human"",
    ""rhymes"": [],
    ""soundsLike"": []
  },
  {
    ""word"": ""humble"",
    ""rhymes"": [
      ""tumble"",
      ""stumble"",
      ""crumble""
    ],
    ""soundsLike"": [
      ""tumble"",
      ""symbol""
    ]
  },
  {
    ""word"": ""humor"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""hammer"",
      ""human""
    ]
  },
  {
    ""word"": ""hundred"",
    ""rhymes"": [],
    ""soundsLike"": []
  },
  {
    ""word"": ""hungry"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""angry"",
      ""kangaroo""
    ]
  },
  {
    ""word"": ""hunt"",
    ""rhymes"": [
      ""front"",
      ""grunt""
    ],
    ""soundsLike"": [
      ""hint"",
      ""hand"",
      ""honey"",
      ""hen""
    ]
  },
  {
    ""word"": ""hurdle"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""turtle""
    ]
  },
  {
    ""word"": ""hurry"",
    ""rhymes"": [
      ""worry""
    ],
    ""soundsLike"": [
      ""worry"",
      ""hurt""
    ]
  },
  {
    ""word"": ""hurt"",
    ""rhymes"": [
      ""desert"",
      ""alert"",
      ""skirt"",
      ""concert"",
      ""divert"",
      ""dirt""
    ],
    ""soundsLike"": [
      ""heart"",
      ""hat"",
      ""hurry"",
      ""height"",
      ""art""
    ]
  },
  {
    ""word"": ""husband"",
    ""rhymes"": [],
    ""soundsLike"": []
  },
  {
    ""word"": ""hybrid"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""habit"",
      ""hard""
    ]
  },
  {
    ""word"": ""ice"",
    ""rhymes"": [
      ""advice"",
      ""device"",
      ""nice"",
      ""price"",
      ""rice"",
      ""dice"",
      ""spice"",
      ""slice"",
      ""twice""
    ],
    ""soundsLike"": [
      ""dice""
    ]
  },
  {
    ""word"": ""icon"",
    ""rhymes"": [],
    ""soundsLike"": []
  },
  {
    ""word"": ""idea"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""idle""
    ]
  },
  {
    ""word"": ""identify"",
    ""rhymes"": [
      ""fly"",
      ""eye"",
      ""high"",
      ""supply"",
      ""dry"",
      ""shy"",
      ""cry"",
      ""clarify"",
      ""try"",
      ""verify"",
      ""deny"",
      ""defy"",
      ""rely"",
      ""modify"",
      ""satisfy"",
      ""spy""
    ],
    ""soundsLike"": []
  },
  {
    ""word"": ""idle"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""aisle""
    ]
  },
  {
    ""word"": ""ignore"",
    ""rhymes"": [
      ""door"",
      ""more"",
      ""core"",
      ""floor"",
      ""before"",
      ""dinosaur"",
      ""outdoor"",
      ""indoor""
    ],
    ""soundsLike"": [
      ""inner"",
      ""indoor"",
      ""cigar""
    ]
  },
  {
    ""word"": ""ill"",
    ""rhymes"": [
      ""will"",
      ""still"",
      ""drill"",
      ""hill"",
      ""skill"",
      ""pill"",
      ""until""
    ],
    ""soundsLike"": [
      ""all"",
      ""oil"",
      ""aisle""
    ]
  },
  {
    ""word"": ""illegal"",
    ""rhymes"": [
      ""eagle"",
      ""legal""
    ],
    ""soundsLike"": [
      ""legal""
    ]
  },
  {
    ""word"": ""illness"",
    ""rhymes"": [],
    ""soundsLike"": []
  },
  {
    ""word"": ""image"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""emerge"",
      ""among"",
      ""damage""
    ]
  },
  {
    ""word"": ""imitate"",
    ""rhymes"": [
      ""state"",
      ""rate"",
      ""plate"",
      ""great"",
      ""mandate"",
      ""estate"",
      ""gate"",
      ""indicate"",
      ""update"",
      ""wait"",
      ""debate""
    ],
    ""soundsLike"": [
      ""estate""
    ]
  },
  {
    ""word"": ""immense"",
    ""rhymes"": [
      ""sense"",
      ""fence"",
      ""defense""
    ],
    ""soundsLike"": [
      ""announce""
    ]
  },
  {
    ""word"": ""immune"",
    ""rhymes"": [
      ""moon"",
      ""spoon"",
      ""soon"",
      ""dune"",
      ""raccoon""
    ],
    ""soundsLike"": [
      ""onion"",
      ""engine"",
      ""insane""
    ]
  },
  {
    ""word"": ""impact"",
    ""rhymes"": [
      ""act"",
      ""abstract"",
      ""enact"",
      ""intact"",
      ""exact"",
      ""attract"",
      ""pact""
    ],
    ""soundsLike"": [
      ""enact"",
      ""intact"",
      ""input"",
      ""impose""
    ]
  },
  {
    ""word"": ""impose"",
    ""rhymes"": [
      ""rose"",
      ""close"",
      ""nose"",
      ""expose"",
      ""oppose""
    ],
    ""soundsLike"": [
      ""oppose"",
      ""empower""
    ]
  },
  {
    ""word"": ""improve"",
    ""rhymes"": [
      ""move"",
      ""approve"",
      ""remove""
    ],
    ""soundsLike"": [
      ""approve"",
      ""embrace""
    ]
  },
  {
    ""word"": ""impulse"",
    ""rhymes"": [
      ""pulse""
    ],
    ""soundsLike"": [
      ""simple"",
      ""impose""
    ]
  },
  {
    ""word"": ""inch"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""any""
    ]
  },
  {
    ""word"": ""include"",
    ""rhymes"": [
      ""food"",
      ""attitude"",
      ""rude"",
      ""exclude""
    ],
    ""soundsLike"": [
      ""exclude""
    ]
  },
  {
    ""word"": ""income"",
    ""rhymes"": [
      ""become"",
      ""come"",
      ""thumb"",
      ""drum"",
      ""dumb""
    ],
    ""soundsLike"": [
      ""become""
    ]
  },
  {
    ""word"": ""increase"",
    ""rhymes"": [
      ""piece"",
      ""peace"",
      ""release"",
      ""nice"",
      ""police"",
      ""decrease""
    ],
    ""soundsLike"": [
      ""across"",
      ""decrease"",
      ""erase"",
      ""enrich""
    ]
  },
  {
    ""word"": ""index"",
    ""rhymes"": [],
    ""soundsLike"": []
  },
  {
    ""word"": ""indicate"",
    ""rhymes"": [
      ""state"",
      ""rate"",
      ""plate"",
      ""great"",
      ""mandate"",
      ""estate"",
      ""gate"",
      ""update"",
      ""wait"",
      ""debate""
    ],
    ""soundsLike"": [
      ""intact"",
      ""index"",
      ""conduct"",
      ""enact"",
      ""addict""
    ]
  },
  {
    ""word"": ""indoor"",
    ""rhymes"": [
      ""door"",
      ""more"",
      ""core"",
      ""floor"",
      ""before"",
      ""ignore"",
      ""dinosaur"",
      ""outdoor""
    ],
    ""soundsLike"": [
      ""entire"",
      ""under"",
      ""endorse""
    ]
  },
  {
    ""word"": ""industry"",
    ""rhymes"": [],
    ""soundsLike"": []
  },
  {
    ""word"": ""infant"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""ancient""
    ]
  },
  {
    ""word"": ""inflict"",
    ""rhymes"": [
      ""perfect"",
      ""predict"",
      ""addict""
    ],
    ""soundsLike"": [
      ""reflect"",
      ""enact"",
      ""insect"",
      ""inject""
    ]
  },
  {
    ""word"": ""inform"",
    ""rhymes"": [
      ""warm"",
      ""uniform"",
      ""swarm"",
      ""reform""
    ],
    ""soundsLike"": [
      ""unfair"",
      ""reform"",
      ""enforce"",
      ""confirm""
    ]
  },
  {
    ""word"": ""inhale"",
    ""rhymes"": [
      ""scale"",
      ""rail"",
      ""mail"",
      ""whale"",
      ""detail"",
      ""tail"",
      ""sail"",
      ""female"",
      ""unveil""
    ],
    ""soundsLike"": [
      ""enroll"",
      ""until""
    ]
  },
  {
    ""word"": ""inherit"",
    ""rhymes"": [
      ""merit"",
      ""parrot""
    ],
    ""soundsLike"": [
      ""interest""
    ]
  },
  {
    ""word"": ""initial"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""inhale"",
      ""animal"",
      ""emotion"",
      ""denial"",
      ""enable""
    ]
  },
  {
    ""word"": ""inject"",
    ""rhymes"": [
      ""project"",
      ""subject"",
      ""aspect"",
      ""object"",
      ""perfect"",
      ""reflect"",
      ""direct"",
      ""connect"",
      ""expect"",
      ""correct"",
      ""neglect"",
      ""suspect"",
      ""collect"",
      ""select"",
      ""protect"",
      ""reject"",
      ""detect"",
      ""insect""
    ],
    ""soundsLike"": [
      ""insect"",
      ""enact"",
      ""reject"",
      ""intact"",
      ""enjoy"",
      ""connect"",
      ""impact""
    ]
  },
  {
    ""word"": ""injury"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""enjoy"",
      ""century"",
      ""ginger"",
      ""engine"",
      ""entry""
    ]
  },
  {
    ""word"": ""inmate"",
    ""rhymes"": [
      ""state"",
      ""rate"",
      ""plate"",
      ""great"",
      ""mandate"",
      ""estate"",
      ""gate"",
      ""indicate"",
      ""update"",
      ""wait"",
      ""debate""
    ],
    ""soundsLike"": [
      ""enact""
    ]
  },
  {
    ""word"": ""inner"",
    ""rhymes"": [
      ""dinner"",
      ""winner""
    ],
    ""soundsLike"": [
      ""enter"",
      ""owner"",
      ""dinner""
    ]
  },
  {
    ""word"": ""innocent"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""insane"",
      ""infant""
    ]
  },
  {
    ""word"": ""input"",
    ""rhymes"": [
      ""put"",
      ""foot"",
      ""output""
    ],
    ""soundsLike"": [
      ""impact"",
      ""impose""
    ]
  },
  {
    ""word"": ""inquiry"",
    ""rhymes"": [
      ""diary""
    ],
    ""soundsLike"": [
      ""acquire"",
      ""increase"",
      ""injury"",
      ""entire""
    ]
  },
  {
    ""word"": ""insane"",
    ""rhymes"": [
      ""train"",
      ""domain"",
      ""sustain"",
      ""rain"",
      ""again"",
      ""grain"",
      ""obtain"",
      ""brain"",
      ""gain"",
      ""explain"",
      ""main"",
      ""crane"",
      ""remain""
    ],
    ""soundsLike"": []
  },
  {
    ""word"": ""insect"",
    ""rhymes"": [
      ""project"",
      ""subject"",
      ""aspect"",
      ""object"",
      ""perfect"",
      ""reflect"",
      ""direct"",
      ""connect"",
      ""expect"",
      ""correct"",
      ""neglect"",
      ""suspect"",
      ""collect"",
      ""select"",
      ""protect"",
      ""reject"",
      ""detect"",
      ""inject""
    ],
    ""soundsLike"": [
      ""inject"",
      ""enact"",
      ""inside"",
      ""intact""
    ]
  },
  {
    ""word"": ""inside"",
    ""rhymes"": [
      ""side"",
      ""provide"",
      ""ride"",
      ""slide"",
      ""pride"",
      ""tide"",
      ""guide"",
      ""decide"",
      ""divide"",
      ""wide"",
      ""glide"",
      ""outside""
    ],
    ""soundsLike"": [
      ""decide""
    ]
  },
  {
    ""word"": ""inspire"",
    ""rhymes"": [
      ""fire"",
      ""wire"",
      ""require"",
      ""hire"",
      ""retire"",
      ""expire""
    ],
    ""soundsLike"": [
      ""expire"",
      ""empower"",
      ""inside"",
      ""despair"",
      ""entire"",
      ""ensure""
    ]
  },
  {
    ""word"": ""install"",
    ""rhymes"": [
      ""all"",
      ""ball"",
      ""call"",
      ""fall"",
      ""wall"",
      ""recall"",
      ""small"",
      ""alcohol"",
      ""doll"",
      ""crawl""
    ],
    ""soundsLike"": [
      ""until"",
      ""inhale""
    ]
  },
  {
    ""word"": ""intact"",
    ""rhymes"": [
      ""act"",
      ""impact"",
      ""abstract"",
      ""enact"",
      ""exact"",
      ""attract"",
      ""pact""
    ],
    ""soundsLike"": [
      ""enact"",
      ""impact"",
      ""insect"",
      ""inject"",
      ""indicate"",
      ""antique"",
      ""attack"",
      ""addict"",
      ""attract"",
      ""conduct"",
      ""detect""
    ]
  },
  {
    ""word"": ""interest"",
    ""rhymes"": [],
    ""soundsLike"": []
  },
  {
    ""word"": ""into"",
    ""rhymes"": [
      ""blue"",
      ""you"",
      ""view"",
      ""review"",
      ""true"",
      ""two"",
      ""shoe"",
      ""crew"",
      ""tissue"",
      ""few"",
      ""glue"",
      ""bamboo"",
      ""undo"",
      ""tattoo"",
      ""renew"",
      ""canoe"",
      ""zoo""
    ],
    ""soundsLike"": [
      ""undo"",
      ""empty"",
      ""entire""
    ]
  },
  {
    ""word"": ""invest"",
    ""rhymes"": [
      ""test"",
      ""best"",
      ""suggest"",
      ""nest"",
      ""chest"",
      ""west"",
      ""arrest""
    ],
    ""soundsLike"": [
      ""invite""
    ]
  },
  {
    ""word"": ""invite"",
    ""rhymes"": [
      ""light"",
      ""right"",
      ""night"",
      ""write"",
      ""sight"",
      ""flight"",
      ""bright"",
      ""slight"",
      ""height"",
      ""kite"",
      ""excite"",
      ""midnight""
    ],
    ""soundsLike"": [
      ""invest"",
      ""enact""
    ]
  },
  {
    ""word"": ""involve"",
    ""rhymes"": [
      ""evolve"",
      ""solve""
    ],
    ""soundsLike"": [
      ""evolve"",
      ""unveil"",
      ""enroll""
    ]
  },
  {
    ""word"": ""iron"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""arm""
    ]
  },
  {
    ""word"": ""island"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""silent""
    ]
  },
  {
    ""word"": ""isolate"",
    ""rhymes"": [
      ""state"",
      ""rate"",
      ""plate"",
      ""great"",
      ""mandate"",
      ""estate"",
      ""gate"",
      ""indicate"",
      ""update"",
      ""wait"",
      ""debate""
    ],
    ""soundsLike"": [
      ""assault""
    ]
  },
  {
    ""word"": ""issue"",
    ""rhymes"": [
      ""tissue""
    ],
    ""soundsLike"": [
      ""tissue"",
      ""era""
    ]
  },
  {
    ""word"": ""item"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""autumn"",
      ""atom""
    ]
  },
  {
    ""word"": ""ivory"",
    ""rhymes"": [],
    ""soundsLike"": []
  },
  {
    ""word"": ""jacket"",
    ""rhymes"": [
      ""bracket""
    ],
    ""soundsLike"": [
      ""educate""
    ]
  },
  {
    ""word"": ""jaguar"",
    ""rhymes"": [
      ""bar"",
      ""car"",
      ""jar"",
      ""radar"",
      ""guitar"",
      ""seminar"",
      ""cigar""
    ],
    ""soundsLike"": []
  },
  {
    ""word"": ""jar"",
    ""rhymes"": [
      ""bar"",
      ""car"",
      ""radar"",
      ""guitar"",
      ""seminar"",
      ""jaguar"",
      ""cigar""
    ],
    ""soundsLike"": []
  },
  {
    ""word"": ""jazz"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""juice"",
      ""cheese"",
      ""choose""
    ]
  },
  {
    ""word"": ""jealous"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""jelly""
    ]
  },
  {
    ""word"": ""jeans"",
    ""rhymes"": [],
    ""soundsLike"": []
  },
  {
    ""word"": ""jelly"",
    ""rhymes"": [],
    ""soundsLike"": []
  },
  {
    ""word"": ""jewel"",
    ""rhymes"": [
      ""school"",
      ""rule"",
      ""pool"",
      ""cool"",
      ""tool"",
      ""fuel"",
      ""stool"",
      ""cruel"",
      ""mule"",
      ""unusual""
    ],
    ""soundsLike"": [
      ""gentle""
    ]
  },
  {
    ""word"": ""job"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""shop""
    ]
  },
  {
    ""word"": ""join"",
    ""rhymes"": [
      ""coin""
    ],
    ""soundsLike"": [
      ""joy"",
      ""gym""
    ]
  },
  {
    ""word"": ""joke"",
    ""rhymes"": [
      ""oak"",
      ""evoke"",
      ""smoke""
    ],
    ""soundsLike"": [
      ""check"",
      ""chalk""
    ]
  },
  {
    ""word"": ""journey"",
    ""rhymes"": [],
    ""soundsLike"": []
  },
  {
    ""word"": ""joy"",
    ""rhymes"": [
      ""employ"",
      ""boy"",
      ""enjoy"",
      ""destroy"",
      ""toy""
    ],
    ""soundsLike"": []
  },
  {
    ""word"": ""judge"",
    ""rhymes"": [],
    ""soundsLike"": []
  },
  {
    ""word"": ""juice"",
    ""rhymes"": [
      ""use"",
      ""abuse"",
      ""produce"",
      ""goose"",
      ""reduce"",
      ""excuse""
    ],
    ""soundsLike"": [
      ""jazz"",
      ""use"",
      ""goose"",
      ""chase""
    ]
  },
  {
    ""word"": ""jump"",
    ""rhymes"": [
      ""clump""
    ],
    ""soundsLike"": [
      ""gym""
    ]
  },
  {
    ""word"": ""jungle"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""gentle""
    ]
  },
  {
    ""word"": ""junior"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""ginger"",
      ""senior""
    ]
  },
  {
    ""word"": ""junk"",
    ""rhymes"": [
      ""chunk""
    ],
    ""soundsLike"": [
      ""chunk""
    ]
  },
  {
    ""word"": ""just"",
    ""rhymes"": [
      ""list"",
      ""trust"",
      ""robust"",
      ""twist"",
      ""dust"",
      ""assist"",
      ""exist"",
      ""must"",
      ""adjust"",
      ""resist"",
      ""enlist"",
      ""wrist""
    ],
    ""soundsLike"": [
      ""chest"",
      ""adjust""
    ]
  },
  {
    ""word"": ""kangaroo"",
    ""rhymes"": [
      ""blue"",
      ""into"",
      ""you"",
      ""view"",
      ""review"",
      ""true"",
      ""two"",
      ""shoe"",
      ""crew"",
      ""tissue"",
      ""few"",
      ""glue"",
      ""bamboo"",
      ""undo"",
      ""tattoo"",
      ""renew"",
      ""canoe"",
      ""zoo""
    ],
    ""soundsLike"": [
      ""camera"",
      ""congress"",
      ""hungry"",
      ""angry""
    ]
  },
  {
    ""word"": ""keen"",
    ""rhymes"": [
      ""mean"",
      ""green"",
      ""bean"",
      ""machine"",
      ""screen"",
      ""clean"",
      ""between"",
      ""scene"",
      ""marine""
    ],
    ""soundsLike"": [
      ""can"",
      ""coin"",
      ""calm""
    ]
  },
  {
    ""word"": ""keep"",
    ""rhymes"": [
      ""sleep"",
      ""cheap""
    ],
    ""soundsLike"": [
      ""cup"",
      ""key""
    ]
  },
  {
    ""word"": ""ketchup"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""cup""
    ]
  },
  {
    ""word"": ""key"",
    ""rhymes"": [
      ""tree"",
      ""sea"",
      ""flee"",
      ""degree"",
      ""debris"",
      ""agree"",
      ""fee"",
      ""ski"",
      ""three"",
      ""pony"",
      ""knee""
    ],
    ""soundsLike"": []
  },
  {
    ""word"": ""kick"",
    ""rhymes"": [
      ""stick"",
      ""trick"",
      ""quick"",
      ""sick"",
      ""click"",
      ""brick"",
      ""picnic""
    ],
    ""soundsLike"": [
      ""cake"",
      ""cook"",
      ""click""
    ]
  },
  {
    ""word"": ""kid"",
    ""rhymes"": [
      ""bid"",
      ""good"",
      ""grid""
    ],
    ""soundsLike"": [
      ""code"",
      ""kit"",
      ""cat"",
      ""caught""
    ]
  },
  {
    ""word"": ""kidney"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""canoe""
    ]
  },
  {
    ""word"": ""kind"",
    ""rhymes"": [
      ""bind"",
      ""mind"",
      ""find"",
      ""blind"",
      ""behind"",
      ""remind""
    ],
    ""soundsLike"": [
      ""can"",
      ""candy""
    ]
  },
  {
    ""word"": ""kingdom"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""kitten""
    ]
  },
  {
    ""word"": ""kiss"",
    ""rhymes"": [
      ""miss"",
      ""this"",
      ""dismiss""
    ],
    ""soundsLike"": [
      ""case"",
      ""cause"",
      ""guess""
    ]
  },
  {
    ""word"": ""kit"",
    ""rhymes"": [
      ""fit"",
      ""split"",
      ""grit"",
      ""quit"",
      ""submit"",
      ""admit"",
      ""permit"",
      ""omit""
    ],
    ""soundsLike"": [
      ""cat"",
      ""kid"",
      ""caught"",
      ""kite"",
      ""code""
    ]
  },
  {
    ""word"": ""kitchen"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""caution"",
      ""kitten"",
      ""cushion"",
      ""cousin"",
      ""cannon""
    ]
  },
  {
    ""word"": ""kite"",
    ""rhymes"": [
      ""light"",
      ""right"",
      ""night"",
      ""write"",
      ""sight"",
      ""flight"",
      ""bright"",
      ""slight"",
      ""height"",
      ""invite"",
      ""excite"",
      ""midnight""
    ],
    ""soundsLike"": [
      ""cat"",
      ""caught"",
      ""kit"",
      ""code"",
      ""coyote""
    ]
  },
  {
    ""word"": ""kitten"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""cotton"",
      ""kitchen"",
      ""curtain"",
      ""cousin""
    ]
  },
  {
    ""word"": ""kiwi"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""carry""
    ]
  },
  {
    ""word"": ""knee"",
    ""rhymes"": [
      ""tree"",
      ""sea"",
      ""key"",
      ""flee"",
      ""degree"",
      ""debris"",
      ""agree"",
      ""fee"",
      ""ski"",
      ""three"",
      ""pony""
    ],
    ""soundsLike"": [
      ""now"",
      ""know"",
      ""any""
    ]
  },
  {
    ""word"": ""knife"",
    ""rhymes"": [
      ""life"",
      ""wife""
    ],
    ""soundsLike"": [
      ""wife"",
      ""enough"",
      ""knee""
    ]
  },
  {
    ""word"": ""knock"",
    ""rhymes"": [
      ""stock"",
      ""talk"",
      ""lock"",
      ""clock"",
      ""walk"",
      ""shock"",
      ""hawk"",
      ""flock"",
      ""chalk"",
      ""sock"",
      ""unlock""
    ],
    ""soundsLike"": [
      ""neck""
    ]
  },
  {
    ""word"": ""know"",
    ""rhymes"": [
      ""snow"",
      ""throw"",
      ""hello"",
      ""grow"",
      ""shadow"",
      ""potato"",
      ""toe"",
      ""glow"",
      ""slow"",
      ""tomato"",
      ""below"",
      ""radio"",
      ""meadow"",
      ""tomorrow"",
      ""borrow"",
      ""photo"",
      ""tornado"",
      ""buffalo""
    ],
    ""soundsLike"": [
      ""now"",
      ""knee""
    ]
  },
  {
    ""word"": ""lab"",
    ""rhymes"": [
      ""grab"",
      ""slab""
    ],
    ""soundsLike"": [
      ""loop"",
      ""slab"",
      ""club"",
      ""globe"",
      ""clap""
    ]
  },
  {
    ""word"": ""label"",
    ""rhymes"": [
      ""table"",
      ""enable"",
      ""able"",
      ""stable"",
      ""cable"",
      ""unable""
    ],
    ""soundsLike"": [
      ""loyal"",
      ""table"",
      ""cable""
    ]
  },
  {
    ""word"": ""labor"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""layer"",
      ""lab""
    ]
  },
  {
    ""word"": ""ladder"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""leader"",
      ""later"",
      ""letter"",
      ""lady""
    ]
  },
  {
    ""word"": ""lady"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""ladder""
    ]
  },
  {
    ""word"": ""lake"",
    ""rhymes"": [
      ""awake"",
      ""cake"",
      ""make"",
      ""snake"",
      ""mistake"",
      ""steak""
    ],
    ""soundsLike"": [
      ""like"",
      ""lock"",
      ""leg""
    ]
  },
  {
    ""word"": ""lamp"",
    ""rhymes"": [
      ""camp"",
      ""stamp"",
      ""ramp"",
      ""damp""
    ],
    ""soundsLike"": [
      ""lab"",
      ""clump""
    ]
  },
  {
    ""word"": ""language"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""luggage""
    ]
  },
  {
    ""word"": ""laptop"",
    ""rhymes"": [
      ""top"",
      ""shop"",
      ""drop"",
      ""crop"",
      ""swap""
    ],
    ""soundsLike"": []
  },
  {
    ""word"": ""large"",
    ""rhymes"": [
      ""charge""
    ],
    ""soundsLike"": [
      ""charge"",
      ""layer""
    ]
  },
  {
    ""word"": ""later"",
    ""rhymes"": [
      ""crater"",
      ""elevator""
    ],
    ""soundsLike"": [
      ""letter"",
      ""leader"",
      ""ladder"",
      ""layer""
    ]
  },
  {
    ""word"": ""latin"",
    ""rhymes"": [],
    ""soundsLike"": []
  },
  {
    ""word"": ""laugh"",
    ""rhymes"": [
      ""staff"",
      ""half"",
      ""giraffe""
    ],
    ""soundsLike"": [
      ""life"",
      ""leaf"",
      ""half"",
      ""cliff""
    ]
  },
  {
    ""word"": ""laundry"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""lottery""
    ]
  },
  {
    ""word"": ""lava"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""leave""
    ]
  },
  {
    ""word"": ""law"",
    ""rhymes"": [
      ""draw"",
      ""raw"",
      ""claw""
    ],
    ""soundsLike"": [
      ""claw""
    ]
  },
  {
    ""word"": ""lawn"",
    ""rhymes"": [
      ""upon"",
      ""dawn"",
      ""spawn"",
      ""salon""
    ],
    ""soundsLike"": [
      ""long"",
      ""loan"",
      ""learn"",
      ""law""
    ]
  },
  {
    ""word"": ""lawsuit"",
    ""rhymes"": [
      ""suit"",
      ""shoot"",
      ""route"",
      ""fruit"",
      ""minute"",
      ""execute"",
      ""cute"",
      ""salute""
    ],
    ""soundsLike"": [
      ""list""
    ]
  },
  {
    ""word"": ""layer"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""liar"",
      ""later"",
      ""labor""
    ]
  },
  {
    ""word"": ""lazy"",
    ""rhymes"": [
      ""crazy""
    ],
    ""soundsLike"": [
      ""lady""
    ]
  },
  {
    ""word"": ""leader"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""ladder"",
      ""later"",
      ""letter"",
      ""lady""
    ]
  },
  {
    ""word"": ""leaf"",
    ""rhymes"": [
      ""relief"",
      ""brief"",
      ""chief"",
      ""grief"",
      ""beef""
    ],
    ""soundsLike"": [
      ""life"",
      ""laugh"",
      ""leave"",
      ""chief"",
      ""cliff""
    ]
  },
  {
    ""word"": ""learn"",
    ""rhymes"": [
      ""turn"",
      ""return"",
      ""churn"",
      ""earn""
    ],
    ""soundsLike"": [
      ""lawn"",
      ""churn""
    ]
  },
  {
    ""word"": ""leave"",
    ""rhymes"": [
      ""naive"",
      ""believe"",
      ""achieve"",
      ""receive""
    ],
    ""soundsLike"": [
      ""love"",
      ""live"",
      ""leaf""
    ]
  },
  {
    ""word"": ""lecture"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""leisure""
    ]
  },
  {
    ""word"": ""left"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""lift"",
      ""laugh"",
      ""leaf""
    ]
  },
  {
    ""word"": ""leg"",
    ""rhymes"": [
      ""egg""
    ],
    ""soundsLike"": [
      ""like"",
      ""lake"",
      ""lock""
    ]
  },
  {
    ""word"": ""legal"",
    ""rhymes"": [
      ""eagle"",
      ""illegal""
    ],
    ""soundsLike"": [
      ""local"",
      ""illegal"",
      ""loyal""
    ]
  },
  {
    ""word"": ""legend"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""lend""
    ]
  },
  {
    ""word"": ""leisure"",
    ""rhymes"": [
      ""measure""
    ],
    ""soundsLike"": [
      ""letter"",
      ""later""
    ]
  },
  {
    ""word"": ""lemon"",
    ""rhymes"": [],
    ""soundsLike"": []
  },
  {
    ""word"": ""lend"",
    ""rhymes"": [
      ""end"",
      ""friend"",
      ""attend"",
      ""trend"",
      ""extend"",
      ""spend"",
      ""depend"",
      ""weekend""
    ],
    ""soundsLike"": [
      ""island""
    ]
  },
  {
    ""word"": ""length"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""link""
    ]
  },
  {
    ""word"": ""lens"",
    ""rhymes"": [],
    ""soundsLike"": []
  },
  {
    ""word"": ""leopard"",
    ""rhymes"": [],
    ""soundsLike"": []
  },
  {
    ""word"": ""lesson"",
    ""rhymes"": [],
    ""soundsLike"": []
  },
  {
    ""word"": ""letter"",
    ""rhymes"": [
      ""better""
    ],
    ""soundsLike"": [
      ""later"",
      ""leader"",
      ""ladder"",
      ""leisure""
    ]
  },
  {
    ""word"": ""level"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""loyal"",
      ""lava"",
      ""little""
    ]
  },
  {
    ""word"": ""liar"",
    ""rhymes"": [
      ""fire"",
      ""wire"",
      ""acquire"",
      ""require"",
      ""hire"",
      ""entire"",
      ""retire"",
      ""buyer""
    ],
    ""soundsLike"": [
      ""layer""
    ]
  },
  {
    ""word"": ""liberty"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""leopard""
    ]
  },
  {
    ""word"": ""library"",
    ""rhymes"": [
      ""cherry"",
      ""carry"",
      ""primary"",
      ""very"",
      ""ordinary"",
      ""merry"",
      ""february""
    ],
    ""soundsLike"": [
      ""liberty""
    ]
  },
  {
    ""word"": ""license"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""lesson"",
      ""lion""
    ]
  },
  {
    ""word"": ""life"",
    ""rhymes"": [
      ""knife"",
      ""wife""
    ],
    ""soundsLike"": [
      ""laugh"",
      ""leaf"",
      ""live"",
      ""wife"",
      ""cliff""
    ]
  },
  {
    ""word"": ""lift"",
    ""rhymes"": [
      ""shift"",
      ""drift"",
      ""gift"",
      ""swift""
    ],
    ""soundsLike"": [
      ""left"",
      ""shift"",
      ""laugh"",
      ""leaf""
    ]
  },
  {
    ""word"": ""light"",
    ""rhymes"": [
      ""right"",
      ""night"",
      ""write"",
      ""sight"",
      ""flight"",
      ""bright"",
      ""slight"",
      ""height"",
      ""kite"",
      ""invite"",
      ""excite"",
      ""midnight""
    ],
    ""soundsLike"": [
      ""flight""
    ]
  },
  {
    ""word"": ""like"",
    ""rhymes"": [
      ""strike"",
      ""spike"",
      ""bike""
    ],
    ""soundsLike"": [
      ""lake"",
      ""lock"",
      ""leg""
    ]
  },
  {
    ""word"": ""limb"",
    ""rhymes"": [
      ""swim"",
      ""trim"",
      ""slim"",
      ""gym""
    ],
    ""soundsLike"": [
      ""lawn""
    ]
  },
  {
    ""word"": ""limit"",
    ""rhymes"": [],
    ""soundsLike"": []
  },
  {
    ""word"": ""link"",
    ""rhymes"": [
      ""drink"",
      ""pink"",
      ""wink""
    ],
    ""soundsLike"": [
      ""wink"",
      ""length"",
      ""lake"",
      ""chunk""
    ]
  },
  {
    ""word"": ""lion"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""alien""
    ]
  },
  {
    ""word"": ""liquid"",
    ""rhymes"": [],
    ""soundsLike"": []
  },
  {
    ""word"": ""list"",
    ""rhymes"": [
      ""just"",
      ""twist"",
      ""assist"",
      ""exist"",
      ""resist"",
      ""enlist"",
      ""wrist""
    ],
    ""soundsLike"": []
  },
  {
    ""word"": ""little"",
    ""rhymes"": [
      ""hospital""
    ],
    ""soundsLike"": [
      ""loyal""
    ]
  },
  {
    ""word"": ""live"",
    ""rhymes"": [
      ""drive"",
      ""give"",
      ""thrive"",
      ""derive"",
      ""arrive""
    ],
    ""soundsLike"": [
      ""love"",
      ""leave"",
      ""life"",
      ""glove""
    ]
  },
  {
    ""word"": ""lizard"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""hazard""
    ]
  },
  {
    ""word"": ""load"",
    ""rhymes"": [
      ""code"",
      ""road"",
      ""episode"",
      ""erode""
    ],
    ""soundsLike"": [
      ""loud"",
      ""light""
    ]
  },
  {
    ""word"": ""loan"",
    ""rhymes"": [
      ""bone"",
      ""zone"",
      ""stone"",
      ""tone"",
      ""phone"",
      ""own"",
      ""alone"",
      ""ozone"",
      ""unknown""
    ],
    ""soundsLike"": [
      ""lawn"",
      ""alone"",
      ""limb""
    ]
  },
  {
    ""word"": ""lobster"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""later"",
      ""labor"",
      ""cluster""
    ]
  },
  {
    ""word"": ""local"",
    ""rhymes"": [
      ""vocal""
    ],
    ""soundsLike"": [
      ""legal"",
      ""loyal"",
      ""chuckle""
    ]
  },
  {
    ""word"": ""lock"",
    ""rhymes"": [
      ""stock"",
      ""talk"",
      ""clock"",
      ""walk"",
      ""shock"",
      ""hawk"",
      ""flock"",
      ""knock"",
      ""chalk"",
      ""sock"",
      ""unlock""
    ],
    ""soundsLike"": [
      ""like"",
      ""lake"",
      ""clock"",
      ""flock""
    ]
  },
  {
    ""word"": ""logic"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""lock""
    ]
  },
  {
    ""word"": ""lonely"",
    ""rhymes"": [
      ""only""
    ],
    ""soundsLike"": []
  },
  {
    ""word"": ""long"",
    ""rhymes"": [
      ""strong"",
      ""wrong"",
      ""song""
    ],
    ""soundsLike"": [
      ""lawn"",
      ""law"",
      ""limb""
    ]
  },
  {
    ""word"": ""loop"",
    ""rhymes"": [
      ""group"",
      ""soup""
    ],
    ""soundsLike"": [
      ""lab""
    ]
  },
  {
    ""word"": ""lottery"",
    ""rhymes"": [
      ""pottery""
    ],
    ""soundsLike"": [
      ""later"",
      ""letter""
    ]
  },
  {
    ""word"": ""loud"",
    ""rhymes"": [
      ""cloud"",
      ""proud"",
      ""crowd""
    ],
    ""soundsLike"": [
      ""load"",
      ""light"",
      ""cloud""
    ]
  },
  {
    ""word"": ""lounge"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""lunch"",
      ""lawn""
    ]
  },
  {
    ""word"": ""love"",
    ""rhymes"": [
      ""above"",
      ""dove"",
      ""shove"",
      ""glove""
    ],
    ""soundsLike"": [
      ""live"",
      ""leave"",
      ""glove"",
      ""olive"",
      ""shove""
    ]
  },
  {
    ""word"": ""loyal"",
    ""rhymes"": [
      ""royal""
    ],
    ""soundsLike"": [
      ""little"",
      ""level"",
      ""legal"",
      ""label"",
      ""local""
    ]
  },
  {
    ""word"": ""lucky"",
    ""rhymes"": [],
    ""soundsLike"": []
  },
  {
    ""word"": ""luggage"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""lucky"",
      ""legal""
    ]
  },
  {
    ""word"": ""lumber"",
    ""rhymes"": [
      ""number""
    ],
    ""soundsLike"": [
      ""labor"",
      ""number""
    ]
  },
  {
    ""word"": ""lunar"",
    ""rhymes"": [],
    ""soundsLike"": []
  },
  {
    ""word"": ""lunch"",
    ""rhymes"": [
      ""punch"",
      ""crunch""
    ],
    ""soundsLike"": [
      ""lounge"",
      ""lens""
    ]
  },
  {
    ""word"": ""luxury"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""lecture"",
      ""leisure""
    ]
  },
  {
    ""word"": ""lyrics"",
    ""rhymes"": [],
    ""soundsLike"": []
  },
  {
    ""word"": ""machine"",
    ""rhymes"": [
      ""mean"",
      ""green"",
      ""bean"",
      ""keen"",
      ""screen"",
      ""clean"",
      ""between"",
      ""scene"",
      ""marine""
    ],
    ""soundsLike"": [
      ""motion"",
      ""nation""
    ]
  },
  {
    ""word"": ""mad"",
    ""rhymes"": [
      ""sad"",
      ""add"",
      ""glad"",
      ""dad""
    ],
    ""soundsLike"": [
      ""maid"",
      ""meat""
    ]
  },
  {
    ""word"": ""magic"",
    ""rhymes"": [
      ""tragic""
    ],
    ""soundsLike"": []
  },
  {
    ""word"": ""magnet"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""minute""
    ]
  },
  {
    ""word"": ""maid"",
    ""rhymes"": [
      ""trade"",
      ""blade"",
      ""parade"",
      ""fade"",
      ""afraid"",
      ""decade"",
      ""upgrade""
    ],
    ""soundsLike"": [
      ""mad"",
      ""meat""
    ]
  },
  {
    ""word"": ""mail"",
    ""rhymes"": [
      ""scale"",
      ""rail"",
      ""whale"",
      ""detail"",
      ""tail"",
      ""sail"",
      ""female"",
      ""unveil"",
      ""inhale""
    ],
    ""soundsLike"": []
  },
  {
    ""word"": ""main"",
    ""rhymes"": [
      ""train"",
      ""domain"",
      ""sustain"",
      ""rain"",
      ""again"",
      ""grain"",
      ""obtain"",
      ""brain"",
      ""gain"",
      ""explain"",
      ""crane"",
      ""insane"",
      ""remain""
    ],
    ""soundsLike"": [
      ""man"",
      ""mean"",
      ""moon""
    ]
  },
  {
    ""word"": ""major"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""measure"",
      ""nature""
    ]
  },
  {
    ""word"": ""make"",
    ""rhymes"": [
      ""awake"",
      ""cake"",
      ""snake"",
      ""mistake"",
      ""lake"",
      ""steak""
    ],
    ""soundsLike"": []
  },
  {
    ""word"": ""mammal"",
    ""rhymes"": [],
    ""soundsLike"": []
  },
  {
    ""word"": ""man"",
    ""rhymes"": [
      ""can"",
      ""fan"",
      ""scan"",
      ""van""
    ],
    ""soundsLike"": [
      ""mean"",
      ""moon"",
      ""main""
    ]
  },
  {
    ""word"": ""manage"",
    ""rhymes"": [],
    ""soundsLike"": []
  },
  {
    ""word"": ""mandate"",
    ""rhymes"": [
      ""state"",
      ""rate"",
      ""plate"",
      ""great"",
      ""estate"",
      ""gate"",
      ""indicate"",
      ""update"",
      ""wait"",
      ""debate""
    ],
    ""soundsLike"": []
  },
  {
    ""word"": ""mango"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""monkey""
    ]
  },
  {
    ""word"": ""mansion"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""mention"",
      ""mountain"",
      ""motion"",
      ""machine""
    ]
  },
  {
    ""word"": ""manual"",
    ""rhymes"": [
      ""annual""
    ],
    ""soundsLike"": [
      ""annual"",
      ""minute"",
      ""menu"",
      ""mammal""
    ]
  },
  {
    ""word"": ""maple"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""mobile"",
      ""noble""
    ]
  },
  {
    ""word"": ""marble"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""moral"",
      ""mobile"",
      ""maple""
    ]
  },
  {
    ""word"": ""march"",
    ""rhymes"": [
      ""arch""
    ],
    ""soundsLike"": [
      ""more""
    ]
  },
  {
    ""word"": ""margin"",
    ""rhymes"": [],
    ""soundsLike"": []
  },
  {
    ""word"": ""marine"",
    ""rhymes"": [
      ""mean"",
      ""green"",
      ""bean"",
      ""machine"",
      ""keen"",
      ""screen"",
      ""clean"",
      ""between"",
      ""scene""
    ],
    ""soundsLike"": [
      ""mean"",
      ""man"",
      ""main""
    ]
  },
  {
    ""word"": ""market"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""merit""
    ]
  },
  {
    ""word"": ""marriage"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""message"",
      ""merry""
    ]
  },
  {
    ""word"": ""mask"",
    ""rhymes"": [
      ""ask"",
      ""task""
    ],
    ""soundsLike"": [
      ""mass"",
      ""make""
    ]
  },
  {
    ""word"": ""mass"",
    ""rhymes"": [
      ""grass"",
      ""pass"",
      ""glass"",
      ""gas"",
      ""brass""
    ],
    ""soundsLike"": [
      ""miss"",
      ""mouse"",
      ""match"",
      ""math"",
      ""mask""
    ]
  },
  {
    ""word"": ""master"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""matter""
    ]
  },
  {
    ""word"": ""match"",
    ""rhymes"": [
      ""catch"",
      ""patch""
    ],
    ""soundsLike"": [
      ""much"",
      ""man"",
      ""mass"",
      ""mesh""
    ]
  },
  {
    ""word"": ""material"",
    ""rhymes"": [
      ""cereal""
    ],
    ""soundsLike"": []
  },
  {
    ""word"": ""math"",
    ""rhymes"": [
      ""path""
    ],
    ""soundsLike"": [
      ""myth"",
      ""mass"",
      ""mad""
    ]
  },
  {
    ""word"": ""matrix"",
    ""rhymes"": [],
    ""soundsLike"": []
  },
  {
    ""word"": ""matter"",
    ""rhymes"": [
      ""scatter""
    ],
    ""soundsLike"": [
      ""motor"",
      ""master""
    ]
  },
  {
    ""word"": ""maximum"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""minimum""
    ]
  },
  {
    ""word"": ""maze"",
    ""rhymes"": [
      ""raise"",
      ""phrase"",
      ""praise"",
      ""gaze"",
      ""always""
    ],
    ""soundsLike"": [
      ""mass""
    ]
  },
  {
    ""word"": ""meadow"",
    ""rhymes"": [
      ""know"",
      ""snow"",
      ""throw"",
      ""hello"",
      ""grow"",
      ""shadow"",
      ""potato"",
      ""toe"",
      ""glow"",
      ""slow"",
      ""tomato"",
      ""below"",
      ""radio"",
      ""tomorrow"",
      ""borrow"",
      ""photo"",
      ""tornado"",
      ""buffalo""
    ],
    ""soundsLike"": []
  },
  {
    ""word"": ""mean"",
    ""rhymes"": [
      ""green"",
      ""bean"",
      ""machine"",
      ""keen"",
      ""screen"",
      ""clean"",
      ""between"",
      ""scene"",
      ""marine""
    ],
    ""soundsLike"": [
      ""man"",
      ""moon"",
      ""main"",
      ""marine""
    ]
  },
  {
    ""word"": ""measure"",
    ""rhymes"": [
      ""leisure""
    ],
    ""soundsLike"": [
      ""mother"",
      ""major"",
      ""mirror""
    ]
  },
  {
    ""word"": ""meat"",
    ""rhymes"": [
      ""seat"",
      ""treat"",
      ""street"",
      ""elite"",
      ""sweet"",
      ""retreat"",
      ""athlete"",
      ""wheat"",
      ""repeat""
    ],
    ""soundsLike"": [
      ""mad"",
      ""maid""
    ]
  },
  {
    ""word"": ""mechanic"",
    ""rhymes"": [
      ""panic""
    ],
    ""soundsLike"": []
  },
  {
    ""word"": ""medal"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""middle"",
      ""model"",
      ""metal"",
      ""noodle"",
      ""meadow""
    ]
  },
  {
    ""word"": ""media"",
    ""rhymes"": [],
    ""soundsLike"": []
  },
  {
    ""word"": ""melody"",
    ""rhymes"": [],
    ""soundsLike"": []
  },
  {
    ""word"": ""melt"",
    ""rhymes"": [
      ""belt""
    ],
    ""soundsLike"": []
  },
  {
    ""word"": ""member"",
    ""rhymes"": [
      ""remember"",
      ""december""
    ],
    ""soundsLike"": [
      ""number"",
      ""memory"",
      ""minor"",
      ""lumber""
    ]
  },
  {
    ""word"": ""memory"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""merry""
    ]
  },
  {
    ""word"": ""mention"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""mansion"",
      ""mountain"",
      ""motion"",
      ""machine""
    ]
  },
  {
    ""word"": ""menu"",
    ""rhymes"": [
      ""venue""
    ],
    ""soundsLike"": [
      ""minute""
    ]
  },
  {
    ""word"": ""mercy"",
    ""rhymes"": [],
    ""soundsLike"": []
  },
  {
    ""word"": ""merge"",
    ""rhymes"": [
      ""surge"",
      ""urge"",
      ""emerge""
    ],
    ""soundsLike"": [
      ""emerge"",
      ""match"",
      ""march"",
      ""mesh""
    ]
  },
  {
    ""word"": ""merit"",
    ""rhymes"": [
      ""parrot"",
      ""inherit""
    ],
    ""soundsLike"": [
      ""market""
    ]
  },
  {
    ""word"": ""merry"",
    ""rhymes"": [
      ""cherry"",
      ""carry"",
      ""library"",
      ""primary"",
      ""very"",
      ""ordinary"",
      ""february""
    ],
    ""soundsLike"": [
      ""memory""
    ]
  },
  {
    ""word"": ""mesh"",
    ""rhymes"": [
      ""fresh""
    ],
    ""soundsLike"": [
      ""much"",
      ""match""
    ]
  },
  {
    ""word"": ""message"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""usage"",
      ""marriage"",
      ""muscle"",
      ""manage""
    ]
  },
  {
    ""word"": ""metal"",
    ""rhymes"": [
      ""settle""
    ],
    ""soundsLike"": [
      ""medal"",
      ""middle"",
      ""model""
    ]
  },
  {
    ""word"": ""method"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""merit""
    ]
  },
  {
    ""word"": ""middle"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""medal"",
      ""model"",
      ""metal"",
      ""noodle""
    ]
  },
  {
    ""word"": ""midnight"",
    ""rhymes"": [
      ""light"",
      ""right"",
      ""night"",
      ""write"",
      ""sight"",
      ""flight"",
      ""bright"",
      ""slight"",
      ""height"",
      ""kite"",
      ""invite"",
      ""excite""
    ],
    ""soundsLike"": [
      ""minute""
    ]
  },
  {
    ""word"": ""milk"",
    ""rhymes"": [
      ""silk""
    ],
    ""soundsLike"": []
  },
  {
    ""word"": ""million"",
    ""rhymes"": [],
    ""soundsLike"": []
  },
  {
    ""word"": ""mimic"",
    ""rhymes"": [],
    ""soundsLike"": []
  },
  {
    ""word"": ""mind"",
    ""rhymes"": [
      ""bind"",
      ""find"",
      ""kind"",
      ""blind"",
      ""behind"",
      ""remind""
    ],
    ""soundsLike"": [
      ""man"",
      ""mad"",
      ""main"",
      ""maid""
    ]
  },
  {
    ""word"": ""minimum"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""cinnamon""
    ]
  },
  {
    ""word"": ""minor"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""man"",
      ""main""
    ]
  },
  {
    ""word"": ""minute"",
    ""rhymes"": [
      ""suit"",
      ""shoot"",
      ""route"",
      ""fruit"",
      ""execute"",
      ""cute"",
      ""salute"",
      ""lawsuit""
    ],
    ""soundsLike"": [
      ""midnight"",
      ""magnet""
    ]
  },
  {
    ""word"": ""miracle"",
    ""rhymes"": [],
    ""soundsLike"": []
  },
  {
    ""word"": ""mirror"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""mother"",
      ""merry"",
      ""measure""
    ]
  },
  {
    ""word"": ""misery"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""merry""
    ]
  },
  {
    ""word"": ""miss"",
    ""rhymes"": [
      ""this"",
      ""dismiss"",
      ""kiss""
    ],
    ""soundsLike"": [
      ""mouse"",
      ""mass"",
      ""maze""
    ]
  },
  {
    ""word"": ""mistake"",
    ""rhymes"": [
      ""awake"",
      ""cake"",
      ""make"",
      ""snake"",
      ""lake"",
      ""steak""
    ],
    ""soundsLike"": []
  },
  {
    ""word"": ""mix"",
    ""rhymes"": [
      ""fix"",
      ""six""
    ],
    ""soundsLike"": [
      ""miss"",
      ""next"",
      ""mixed""
    ]
  },
  {
    ""word"": ""mixed"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""next"",
      ""mix"",
      ""must""
    ]
  },
  {
    ""word"": ""mixture"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""mix"",
      ""mixed""
    ]
  },
  {
    ""word"": ""mobile"",
    ""rhymes"": [
      ""noble""
    ],
    ""soundsLike"": [
      ""noble"",
      ""maple"",
      ""marble""
    ]
  },
  {
    ""word"": ""model"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""middle"",
      ""medal"",
      ""metal"",
      ""noodle""
    ]
  },
  {
    ""word"": ""modify"",
    ""rhymes"": [
      ""fly"",
      ""eye"",
      ""high"",
      ""supply"",
      ""identify"",
      ""dry"",
      ""shy"",
      ""cry"",
      ""clarify"",
      ""try"",
      ""verify"",
      ""deny"",
      ""defy"",
      ""rely"",
      ""satisfy"",
      ""spy""
    ],
    ""soundsLike"": [
      ""model"",
      ""media"",
      ""medal""
    ]
  },
  {
    ""word"": ""mom"",
    ""rhymes"": [
      ""bomb"",
      ""palm"",
      ""calm""
    ],
    ""soundsLike"": [
      ""man"",
      ""mean"",
      ""main""
    ]
  },
  {
    ""word"": ""moment"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""minute""
    ]
  },
  {
    ""word"": ""monitor"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""monster"",
      ""minute""
    ]
  },
  {
    ""word"": ""monkey"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""mango""
    ]
  },
  {
    ""word"": ""monster"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""monitor"",
      ""master""
    ]
  },
  {
    ""word"": ""month"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""myth"",
      ""man"",
      ""main""
    ]
  },
  {
    ""word"": ""moon"",
    ""rhymes"": [
      ""spoon"",
      ""soon"",
      ""immune"",
      ""dune"",
      ""raccoon""
    ],
    ""soundsLike"": [
      ""man"",
      ""mean"",
      ""main"",
      ""mom""
    ]
  },
  {
    ""word"": ""moral"",
    ""rhymes"": [
      ""coral""
    ],
    ""soundsLike"": [
      ""marble""
    ]
  },
  {
    ""word"": ""more"",
    ""rhymes"": [
      ""door"",
      ""core"",
      ""floor"",
      ""before"",
      ""ignore"",
      ""dinosaur"",
      ""outdoor"",
      ""indoor""
    ],
    ""soundsLike"": []
  },
  {
    ""word"": ""morning"",
    ""rhymes"": [],
    ""soundsLike"": []
  },
  {
    ""word"": ""mosquito"",
    ""rhymes"": [],
    ""soundsLike"": []
  },
  {
    ""word"": ""mother"",
    ""rhymes"": [
      ""other"",
      ""rather"",
      ""another"",
      ""brother""
    ],
    ""soundsLike"": [
      ""measure"",
      ""neither"",
      ""mirror"",
      ""myth""
    ]
  },
  {
    ""word"": ""motion"",
    ""rhymes"": [
      ""ocean"",
      ""emotion""
    ],
    ""soundsLike"": [
      ""machine"",
      ""emotion"",
      ""nation"",
      ""mention"",
      ""mansion""
    ]
  },
  {
    ""word"": ""motor"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""matter"",
      ""amateur""
    ]
  },
  {
    ""word"": ""mountain"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""mention"",
      ""mansion""
    ]
  },
  {
    ""word"": ""mouse"",
    ""rhymes"": [
      ""blouse""
    ],
    ""soundsLike"": [
      ""miss"",
      ""mass"",
      ""maze""
    ]
  },
  {
    ""word"": ""move"",
    ""rhymes"": [
      ""improve"",
      ""approve"",
      ""remove""
    ],
    ""soundsLike"": [
      ""movie"",
      ""match""
    ]
  },
  {
    ""word"": ""movie"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""move"",
      ""heavy""
    ]
  },
  {
    ""word"": ""much"",
    ""rhymes"": [
      ""such"",
      ""clutch"",
      ""dutch""
    ],
    ""soundsLike"": [
      ""match"",
      ""mesh""
    ]
  },
  {
    ""word"": ""muffin"",
    ""rhymes"": [],
    ""soundsLike"": []
  },
  {
    ""word"": ""mule"",
    ""rhymes"": [
      ""school"",
      ""rule"",
      ""pool"",
      ""cool"",
      ""tool"",
      ""fuel"",
      ""stool"",
      ""cruel"",
      ""jewel""
    ],
    ""soundsLike"": [
      ""mail""
    ]
  },
  {
    ""word"": ""multiply"",
    ""rhymes"": [
      ""fly"",
      ""eye"",
      ""high"",
      ""supply"",
      ""identify"",
      ""dry"",
      ""shy"",
      ""cry"",
      ""clarify"",
      ""try"",
      ""verify"",
      ""deny"",
      ""defy"",
      ""rely"",
      ""modify"",
      ""satisfy"",
      ""spy""
    ],
    ""soundsLike"": []
  },
  {
    ""word"": ""muscle"",
    ""rhymes"": [],
    ""soundsLike"": []
  },
  {
    ""word"": ""museum"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""music""
    ]
  },
  {
    ""word"": ""mushroom"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""machine""
    ]
  },
  {
    ""word"": ""music"",
    ""rhymes"": [],
    ""soundsLike"": []
  },
  {
    ""word"": ""must"",
    ""rhymes"": [
      ""trust"",
      ""robust"",
      ""just"",
      ""dust"",
      ""adjust""
    ],
    ""soundsLike"": [
      ""nest"",
      ""miss""
    ]
  },
  {
    ""word"": ""mutual"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""unusual"",
      ""usual"",
      ""museum"",
      ""mule"",
      ""manual""
    ]
  },
  {
    ""word"": ""myself"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""muscle""
    ]
  },
  {
    ""word"": ""mystery"",
    ""rhymes"": [
      ""history""
    ],
    ""soundsLike"": [
      ""history"",
      ""master"",
      ""misery""
    ]
  },
  {
    ""word"": ""myth"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""math"",
      ""miss""
    ]
  },
  {
    ""word"": ""naive"",
    ""rhymes"": [
      ""leave"",
      ""believe"",
      ""achieve"",
      ""receive""
    ],
    ""soundsLike"": [
      ""knife"",
      ""knee"",
      ""nerve""
    ]
  },
  {
    ""word"": ""name"",
    ""rhymes"": [
      ""game"",
      ""frame"",
      ""claim"",
      ""aim"",
      ""same"",
      ""blame"",
      ""flame"",
      ""fame""
    ],
    ""soundsLike"": []
  },
  {
    ""word"": ""napkin"",
    ""rhymes"": [],
    ""soundsLike"": []
  },
  {
    ""word"": ""narrow"",
    ""rhymes"": [
      ""arrow""
    ],
    ""soundsLike"": [
      ""merry"",
      ""near""
    ]
  },
  {
    ""word"": ""nasty"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""nest"",
      ""master""
    ]
  },
  {
    ""word"": ""nation"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""motion"",
      ""machine""
    ]
  },
  {
    ""word"": ""nature"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""major"",
      ""near"",
      ""neither""
    ]
  },
  {
    ""word"": ""near"",
    ""rhymes"": [
      ""year"",
      ""deer"",
      ""pioneer"",
      ""appear"",
      ""sphere""
    ],
    ""soundsLike"": []
  },
  {
    ""word"": ""neck"",
    ""rhymes"": [
      ""check"",
      ""wreck""
    ],
    ""soundsLike"": [
      ""knock""
    ]
  },
  {
    ""word"": ""need"",
    ""rhymes"": [
      ""feed"",
      ""speed"",
      ""seed""
    ],
    ""soundsLike"": [
      ""knee""
    ]
  },
  {
    ""word"": ""negative"",
    ""rhymes"": [],
    ""soundsLike"": []
  },
  {
    ""word"": ""neglect"",
    ""rhymes"": [
      ""project"",
      ""subject"",
      ""aspect"",
      ""object"",
      ""perfect"",
      ""reflect"",
      ""direct"",
      ""connect"",
      ""expect"",
      ""correct"",
      ""suspect"",
      ""collect"",
      ""select"",
      ""protect"",
      ""reject"",
      ""detect"",
      ""insect"",
      ""inject""
    ],
    ""soundsLike"": [
      ""select"",
      ""collect""
    ]
  },
  {
    ""word"": ""neither"",
    ""rhymes"": [
      ""either""
    ],
    ""soundsLike"": [
      ""mother"",
      ""near"",
      ""another"",
      ""nature""
    ]
  },
  {
    ""word"": ""nephew"",
    ""rhymes"": [],
    ""soundsLike"": []
  },
  {
    ""word"": ""nerve"",
    ""rhymes"": [
      ""curve"",
      ""observe""
    ],
    ""soundsLike"": [
      ""nurse""
    ]
  },
  {
    ""word"": ""nest"",
    ""rhymes"": [
      ""test"",
      ""best"",
      ""suggest"",
      ""chest"",
      ""west"",
      ""arrest"",
      ""invest""
    ],
    ""soundsLike"": [
      ""next"",
      ""must"",
      ""net"",
      ""chest"",
      ""west"",
      ""nut""
    ]
  },
  {
    ""word"": ""net"",
    ""rhymes"": [
      ""upset"",
      ""asset"",
      ""wet"",
      ""forget"",
      ""regret"",
      ""pet"",
      ""sunset""
    ],
    ""soundsLike"": [
      ""nut"",
      ""night"",
      ""note""
    ]
  },
  {
    ""word"": ""network"",
    ""rhymes"": [
      ""work"",
      ""clerk"",
      ""artwork""
    ],
    ""soundsLike"": [
      ""artwork""
    ]
  },
  {
    ""word"": ""neutral"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""noodle""
    ]
  },
  {
    ""word"": ""never"",
    ""rhymes"": [
      ""clever""
    ],
    ""soundsLike"": [
      ""near"",
      ""nature"",
      ""hover""
    ]
  },
  {
    ""word"": ""news"",
    ""rhymes"": [
      ""choose"",
      ""abuse"",
      ""refuse"",
      ""cruise"",
      ""excuse"",
      ""accuse""
    ],
    ""soundsLike"": [
      ""nose"",
      ""noise"",
      ""nice""
    ]
  },
  {
    ""word"": ""next"",
    ""rhymes"": [
      ""text""
    ],
    ""soundsLike"": [
      ""nest"",
      ""mixed"",
      ""text""
    ]
  },
  {
    ""word"": ""nice"",
    ""rhymes"": [
      ""ice"",
      ""advice"",
      ""piece"",
      ""device"",
      ""peace"",
      ""increase"",
      ""release"",
      ""price"",
      ""police"",
      ""rice"",
      ""dice"",
      ""spice"",
      ""slice"",
      ""decrease"",
      ""twice""
    ],
    ""soundsLike"": [
      ""news"",
      ""noise""
    ]
  },
  {
    ""word"": ""night"",
    ""rhymes"": [
      ""light"",
      ""right"",
      ""write"",
      ""sight"",
      ""flight"",
      ""bright"",
      ""slight"",
      ""height"",
      ""kite"",
      ""invite"",
      ""excite"",
      ""midnight""
    ],
    ""soundsLike"": [
      ""note"",
      ""net"",
      ""nut"",
      ""need""
    ]
  },
  {
    ""word"": ""noble"",
    ""rhymes"": [
      ""mobile""
    ],
    ""soundsLike"": [
      ""mobile"",
      ""unable"",
      ""enable""
    ]
  },
  {
    ""word"": ""noise"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""nose"",
      ""news"",
      ""nice""
    ]
  },
  {
    ""word"": ""nominee"",
    ""rhymes"": [
      ""tree"",
      ""sea"",
      ""key"",
      ""flee"",
      ""degree"",
      ""debris"",
      ""agree"",
      ""fee"",
      ""ski"",
      ""three"",
      ""pony"",
      ""knee""
    ],
    ""soundsLike"": []
  },
  {
    ""word"": ""noodle"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""model"",
      ""middle"",
      ""medal"",
      ""neutral""
    ]
  },
  {
    ""word"": ""normal"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""moral"",
      ""mammal""
    ]
  },
  {
    ""word"": ""north"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""near"",
      ""narrow""
    ]
  },
  {
    ""word"": ""nose"",
    ""rhymes"": [
      ""rose"",
      ""close"",
      ""impose"",
      ""expose"",
      ""oppose""
    ],
    ""soundsLike"": [
      ""news"",
      ""noise"",
      ""know"",
      ""nice""
    ]
  },
  {
    ""word"": ""notable"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""noble""
    ]
  },
  {
    ""word"": ""note"",
    ""rhymes"": [
      ""boat"",
      ""promote"",
      ""float"",
      ""quote"",
      ""goat"",
      ""vote"",
      ""devote""
    ],
    ""soundsLike"": [
      ""night"",
      ""net"",
      ""nut"",
      ""know"",
      ""need""
    ]
  },
  {
    ""word"": ""nothing"",
    ""rhymes"": [],
    ""soundsLike"": []
  },
  {
    ""word"": ""notice"",
    ""rhymes"": [],
    ""soundsLike"": []
  },
  {
    ""word"": ""novel"",
    ""rhymes"": [],
    ""soundsLike"": []
  },
  {
    ""word"": ""now"",
    ""rhymes"": [
      ""allow"",
      ""eyebrow""
    ],
    ""soundsLike"": [
      ""know"",
      ""knee""
    ]
  },
  {
    ""word"": ""nuclear"",
    ""rhymes"": [],
    ""soundsLike"": []
  },
  {
    ""word"": ""number"",
    ""rhymes"": [
      ""lumber""
    ],
    ""soundsLike"": [
      ""member"",
      ""lumber""
    ]
  },
  {
    ""word"": ""nurse"",
    ""rhymes"": [
      ""universe"",
      ""purse""
    ],
    ""soundsLike"": [
      ""nice""
    ]
  },
  {
    ""word"": ""nut"",
    ""rhymes"": [
      ""that"",
      ""what"",
      ""robot"",
      ""walnut"",
      ""peanut"",
      ""coconut""
    ],
    ""soundsLike"": [
      ""net"",
      ""night"",
      ""note""
    ]
  },
  {
    ""word"": ""oak"",
    ""rhymes"": [
      ""evoke"",
      ""smoke"",
      ""joke""
    ],
    ""soundsLike"": []
  },
  {
    ""word"": ""obey"",
    ""rhymes"": [
      ""day"",
      ""away"",
      ""way"",
      ""play"",
      ""say"",
      ""survey"",
      ""display"",
      ""stay"",
      ""delay"",
      ""holiday"",
      ""essay"",
      ""clay"",
      ""betray"",
      ""spray"",
      ""they"",
      ""okay"",
      ""tray"",
      ""today""
    ],
    ""soundsLike"": []
  },
  {
    ""word"": ""object"",
    ""rhymes"": [
      ""project"",
      ""subject"",
      ""aspect"",
      ""perfect"",
      ""reflect"",
      ""direct"",
      ""connect"",
      ""expect"",
      ""correct"",
      ""neglect"",
      ""suspect"",
      ""collect"",
      ""select"",
      ""protect"",
      ""reject"",
      ""detect"",
      ""insect"",
      ""inject""
    ],
    ""soundsLike"": [
      ""subject"",
      ""reject"",
      ""inject"",
      ""project"",
      ""educate""
    ]
  },
  {
    ""word"": ""oblige"",
    ""rhymes"": [],
    ""soundsLike"": []
  },
  {
    ""word"": ""obscure"",
    ""rhymes"": [
      ""ensure"",
      ""sure""
    ],
    ""soundsLike"": [
      ""acquire""
    ]
  },
  {
    ""word"": ""observe"",
    ""rhymes"": [
      ""nerve"",
      ""curve""
    ],
    ""soundsLike"": [
      ""above"",
      ""absurd"",
      ""appear"",
      ""approve"",
      ""upper""
    ]
  },
  {
    ""word"": ""obtain"",
    ""rhymes"": [
      ""train"",
      ""domain"",
      ""sustain"",
      ""rain"",
      ""again"",
      ""grain"",
      ""brain"",
      ""gain"",
      ""explain"",
      ""main"",
      ""crane"",
      ""insane"",
      ""remain""
    ],
    ""soundsLike"": []
  },
  {
    ""word"": ""obvious"",
    ""rhymes"": [],
    ""soundsLike"": []
  },
  {
    ""word"": ""occur"",
    ""rhymes"": [
      ""transfer"",
      ""amateur"",
      ""prefer"",
      ""blur""
    ],
    ""soundsLike"": [
      ""eager""
    ]
  },
  {
    ""word"": ""ocean"",
    ""rhymes"": [
      ""motion"",
      ""emotion""
    ],
    ""soundsLike"": [
      ""motion"",
      ""open""
    ]
  },
  {
    ""word"": ""october"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""actor"",
      ""doctor""
    ]
  },
  {
    ""word"": ""odor"",
    ""rhymes"": [],
    ""soundsLike"": []
  },
  {
    ""word"": ""off"",
    ""rhymes"": [],
    ""soundsLike"": []
  },
  {
    ""word"": ""offer"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""off"",
      ""author""
    ]
  },
  {
    ""word"": ""office"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""often""
    ]
  },
  {
    ""word"": ""often"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""orphan"",
      ""office"",
      ""awful""
    ]
  },
  {
    ""word"": ""oil"",
    ""rhymes"": [
      ""foil"",
      ""spoil"",
      ""coil"",
      ""boil""
    ],
    ""soundsLike"": [
      ""all"",
      ""ill"",
      ""aisle"",
      ""boil"",
      ""foil"",
      ""coil""
    ]
  },
  {
    ""word"": ""okay"",
    ""rhymes"": [
      ""day"",
      ""away"",
      ""way"",
      ""play"",
      ""say"",
      ""survey"",
      ""display"",
      ""stay"",
      ""delay"",
      ""holiday"",
      ""essay"",
      ""clay"",
      ""betray"",
      ""spray"",
      ""they"",
      ""tray"",
      ""obey"",
      ""today""
    ],
    ""soundsLike"": [
      ""oak"",
      ""echo""
    ]
  },
  {
    ""word"": ""old"",
    ""rhymes"": [
      ""hold"",
      ""gold"",
      ""fold"",
      ""uphold"",
      ""unfold""
    ],
    ""soundsLike"": [
      ""gold"",
      ""hold"",
      ""fold""
    ]
  },
  {
    ""word"": ""olive"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""glove""
    ]
  },
  {
    ""word"": ""olympic"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""clinic""
    ]
  },
  {
    ""word"": ""omit"",
    ""rhymes"": [
      ""fit"",
      ""split"",
      ""grit"",
      ""quit"",
      ""kit"",
      ""submit"",
      ""admit"",
      ""permit""
    ],
    ""soundsLike"": [
      ""admit""
    ]
  },
  {
    ""word"": ""once"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""one"",
      ""win"",
      ""when""
    ]
  },
  {
    ""word"": ""one"",
    ""rhymes"": [
      ""run"",
      ""gun"",
      ""fun"",
      ""sun"",
      ""someone""
    ],
    ""soundsLike"": [
      ""win"",
      ""when"",
      ""wine"",
      ""wing"",
      ""once""
    ]
  },
  {
    ""word"": ""onion"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""engine"",
      ""unknown"",
      ""immune"",
      ""canyon""
    ]
  },
  {
    ""word"": ""online"",
    ""rhymes"": [
      ""design"",
      ""sign"",
      ""fine"",
      ""wine"",
      ""genuine"",
      ""shine"",
      ""decline"",
      ""define"",
      ""combine""
    ],
    ""soundsLike"": []
  },
  {
    ""word"": ""only"",
    ""rhymes"": [
      ""lonely""
    ],
    ""soundsLike"": [
      ""lonely"",
      ""alley""
    ]
  },
  {
    ""word"": ""open"",
    ""rhymes"": [
      ""reopen""
    ],
    ""soundsLike"": [
      ""upon"",
      ""ocean""
    ]
  },
  {
    ""word"": ""opera"",
    ""rhymes"": [],
    ""soundsLike"": []
  },
  {
    ""word"": ""opinion"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""onion"",
      ""abandon"",
      ""canyon"",
      ""reunion""
    ]
  },
  {
    ""word"": ""oppose"",
    ""rhymes"": [
      ""rose"",
      ""close"",
      ""nose"",
      ""impose"",
      ""expose""
    ],
    ""soundsLike"": [
      ""appear"",
      ""impose""
    ]
  },
  {
    ""word"": ""option"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""open"",
      ""ocean"",
      ""caution"",
      ""opera"",
      ""auction""
    ]
  },
  {
    ""word"": ""orange"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""fringe"",
      ""crunch""
    ]
  },
  {
    ""word"": ""orbit"",
    ""rhymes"": [],
    ""soundsLike"": []
  },
  {
    ""word"": ""orchard"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""forward"",
      ""awkward"",
      ""arch""
    ]
  },
  {
    ""word"": ""order"",
    ""rhymes"": [
      ""disorder"",
      ""border""
    ],
    ""soundsLike"": [
      ""border"",
      ""quarter""
    ]
  },
  {
    ""word"": ""ordinary"",
    ""rhymes"": [
      ""cherry"",
      ""carry"",
      ""library"",
      ""primary"",
      ""very"",
      ""merry"",
      ""february""
    ],
    ""soundsLike"": []
  },
  {
    ""word"": ""organ"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""bargain""
    ]
  },
  {
    ""word"": ""orient"",
    ""rhymes"": [
      ""present"",
      ""rent"",
      ""prevent"",
      ""segment"",
      ""tent"",
      ""cement"",
      ""frequent""
    ],
    ""soundsLike"": []
  },
  {
    ""word"": ""original"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""ritual"",
      ""region""
    ]
  },
  {
    ""word"": ""orphan"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""often"",
      ""organ""
    ]
  },
  {
    ""word"": ""ostrich"",
    ""rhymes"": [],
    ""soundsLike"": []
  },
  {
    ""word"": ""other"",
    ""rhymes"": [
      ""rather"",
      ""another"",
      ""mother"",
      ""brother""
    ],
    ""soundsLike"": [
      ""either"",
      ""author"",
      ""mother"",
      ""rather"",
      ""air"",
      ""enter"",
      ""error""
    ]
  },
  {
    ""word"": ""outdoor"",
    ""rhymes"": [
      ""door"",
      ""more"",
      ""core"",
      ""floor"",
      ""before"",
      ""ignore"",
      ""dinosaur"",
      ""indoor""
    ],
    ""soundsLike"": [
      ""outer"",
      ""indoor""
    ]
  },
  {
    ""word"": ""outer"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""hour"",
      ""odor""
    ]
  },
  {
    ""word"": ""output"",
    ""rhymes"": [
      ""put"",
      ""foot"",
      ""input""
    ],
    ""soundsLike"": [
      ""input"",
      ""audit""
    ]
  },
  {
    ""word"": ""outside"",
    ""rhymes"": [
      ""side"",
      ""provide"",
      ""ride"",
      ""slide"",
      ""pride"",
      ""tide"",
      ""guide"",
      ""decide"",
      ""divide"",
      ""wide"",
      ""glide"",
      ""inside""
    ],
    ""soundsLike"": [
      ""inside"",
      ""decide"",
      ""acid""
    ]
  },
  {
    ""word"": ""oval"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""evil"",
      ""awful"",
      ""aisle"",
      ""civil""
    ]
  },
  {
    ""word"": ""oven"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""seven""
    ]
  },
  {
    ""word"": ""over"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""odor"",
      ""air""
    ]
  },
  {
    ""word"": ""own"",
    ""rhymes"": [
      ""bone"",
      ""zone"",
      ""stone"",
      ""tone"",
      ""loan"",
      ""phone"",
      ""alone"",
      ""ozone"",
      ""unknown""
    ],
    ""soundsLike"": []
  },
  {
    ""word"": ""owner"",
    ""rhymes"": [
      ""donor""
    ],
    ""soundsLike"": [
      ""enter"",
      ""inner"",
      ""own"",
      ""donor""
    ]
  },
  {
    ""word"": ""oxygen"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""auction""
    ]
  },
  {
    ""word"": ""oyster"",
    ""rhymes"": [],
    ""soundsLike"": []
  },
  {
    ""word"": ""ozone"",
    ""rhymes"": [
      ""bone"",
      ""zone"",
      ""stone"",
      ""tone"",
      ""loan"",
      ""phone"",
      ""own"",
      ""alone"",
      ""unknown""
    ],
    ""soundsLike"": [
      ""ocean""
    ]
  },
  {
    ""word"": ""pact"",
    ""rhymes"": [
      ""act"",
      ""impact"",
      ""abstract"",
      ""enact"",
      ""intact"",
      ""exact"",
      ""attract""
    ],
    ""soundsLike"": []
  },
  {
    ""word"": ""paddle"",
    ""rhymes"": [
      ""saddle""
    ],
    ""soundsLike"": [
      ""battle""
    ]
  },
  {
    ""word"": ""page"",
    ""rhymes"": [
      ""gauge"",
      ""age"",
      ""engage"",
      ""stage"",
      ""cage"",
      ""wage""
    ],
    ""soundsLike"": [
      ""push"",
      ""pitch"",
      ""patch"",
      ""badge"",
      ""wage"",
      ""pig""
    ]
  },
  {
    ""word"": ""pair"",
    ""rhymes"": [
      ""air"",
      ""chair"",
      ""wear"",
      ""square"",
      ""share"",
      ""despair"",
      ""there"",
      ""hair"",
      ""spare"",
      ""aware"",
      ""where"",
      ""rare"",
      ""swear"",
      ""pear"",
      ""affair"",
      ""repair"",
      ""prepare"",
      ""glare"",
      ""scare"",
      ""warfare"",
      ""unfair"",
      ""unaware""
    ],
    ""soundsLike"": [
      ""pear"",
      ""power"",
      ""pepper""
    ]
  },
  {
    ""word"": ""palace"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""police""
    ]
  },
  {
    ""word"": ""palm"",
    ""rhymes"": [
      ""bomb"",
      ""calm"",
      ""mom""
    ],
    ""soundsLike"": [
      ""bomb"",
      ""pen""
    ]
  },
  {
    ""word"": ""panda"",
    ""rhymes"": [],
    ""soundsLike"": []
  },
  {
    ""word"": ""panel"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""pencil""
    ]
  },
  {
    ""word"": ""panic"",
    ""rhymes"": [
      ""mechanic""
    ],
    ""soundsLike"": [
      ""picnic"",
      ""panel""
    ]
  },
  {
    ""word"": ""panther"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""banner"",
      ""path""
    ]
  },
  {
    ""word"": ""paper"",
    ""rhymes"": [
      ""vapor""
    ],
    ""soundsLike"": [
      ""pepper"",
      ""pair"",
      ""pear""
    ]
  },
  {
    ""word"": ""parade"",
    ""rhymes"": [
      ""trade"",
      ""blade"",
      ""fade"",
      ""afraid"",
      ""decade"",
      ""upgrade"",
      ""maid""
    ],
    ""soundsLike"": [
      ""proud"",
      ""pride"",
      ""bread""
    ]
  },
  {
    ""word"": ""parent"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""parrot"",
      ""peasant"",
      ""print"",
      ""patient""
    ]
  },
  {
    ""word"": ""park"",
    ""rhymes"": [
      ""embark""
    ],
    ""soundsLike"": [
      ""power""
    ]
  },
  {
    ""word"": ""parrot"",
    ""rhymes"": [
      ""merit"",
      ""inherit""
    ],
    ""soundsLike"": [
      ""parent"",
      ""spirit""
    ]
  },
  {
    ""word"": ""party"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""poverty""
    ]
  },
  {
    ""word"": ""pass"",
    ""rhymes"": [
      ""grass"",
      ""glass"",
      ""mass"",
      ""gas"",
      ""brass""
    ],
    ""soundsLike"": [
      ""peace"",
      ""piece"",
      ""path"",
      ""pause"",
      ""patch""
    ]
  },
  {
    ""word"": ""patch"",
    ""rhymes"": [
      ""catch"",
      ""match""
    ],
    ""soundsLike"": [
      ""pitch"",
      ""pass"",
      ""page"",
      ""push"",
      ""beach"",
      ""badge""
    ]
  },
  {
    ""word"": ""path"",
    ""rhymes"": [
      ""math""
    ],
    ""soundsLike"": [
      ""pass"",
      ""palm""
    ]
  },
  {
    ""word"": ""patient"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""parent"",
      ""peasant"",
      ""payment"",
      ""pigeon""
    ]
  },
  {
    ""word"": ""patrol"",
    ""rhymes"": [
      ""control"",
      ""hole"",
      ""pole"",
      ""soul"",
      ""enroll""
    ],
    ""soundsLike"": [
      ""pottery""
    ]
  },
  {
    ""word"": ""pattern"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""pottery""
    ]
  },
  {
    ""word"": ""pause"",
    ""rhymes"": [
      ""cause"",
      ""because""
    ],
    ""soundsLike"": [
      ""peace"",
      ""pass"",
      ""piece""
    ]
  },
  {
    ""word"": ""pave"",
    ""rhymes"": [
      ""wave"",
      ""brave"",
      ""save"",
      ""cave"",
      ""behave""
    ],
    ""soundsLike"": [
      ""wave"",
      ""page"",
      ""patch""
    ]
  },
  {
    ""word"": ""payment"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""patient"",
      ""peasant"",
      ""peanut""
    ]
  },
  {
    ""word"": ""peace"",
    ""rhymes"": [
      ""piece"",
      ""increase"",
      ""release"",
      ""nice"",
      ""police"",
      ""decrease""
    ],
    ""soundsLike"": [
      ""piece"",
      ""pass"",
      ""pause""
    ]
  },
  {
    ""word"": ""peanut"",
    ""rhymes"": [
      ""that"",
      ""nut"",
      ""what"",
      ""robot"",
      ""walnut"",
      ""coconut""
    ],
    ""soundsLike"": [
      ""point"",
      ""planet"",
      ""poet""
    ]
  },
  {
    ""word"": ""pear"",
    ""rhymes"": [
      ""air"",
      ""chair"",
      ""wear"",
      ""square"",
      ""share"",
      ""despair"",
      ""there"",
      ""hair"",
      ""spare"",
      ""aware"",
      ""pair"",
      ""where"",
      ""rare"",
      ""swear"",
      ""affair"",
      ""repair"",
      ""prepare"",
      ""glare"",
      ""scare"",
      ""warfare"",
      ""unfair"",
      ""unaware""
    ],
    ""soundsLike"": [
      ""pair"",
      ""power"",
      ""pepper""
    ]
  },
  {
    ""word"": ""peasant"",
    ""rhymes"": [
      ""present""
    ],
    ""soundsLike"": [
      ""present"",
      ""parent"",
      ""patient"",
      ""parrot"",
      ""payment""
    ]
  },
  {
    ""word"": ""pelican"",
    ""rhymes"": [],
    ""soundsLike"": []
  },
  {
    ""word"": ""pen"",
    ""rhymes"": [
      ""then"",
      ""again"",
      ""hen"",
      ""when"",
      ""ten""
    ],
    ""soundsLike"": [
      ""palm""
    ]
  },
  {
    ""word"": ""penalty"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""peanut"",
      ""panel""
    ]
  },
  {
    ""word"": ""pencil"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""panel"",
      ""puzzle""
    ]
  },
  {
    ""word"": ""people"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""pupil"",
      ""bubble""
    ]
  },
  {
    ""word"": ""pepper"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""paper"",
      ""pair"",
      ""pear"",
      ""puppy""
    ]
  },
  {
    ""word"": ""perfect"",
    ""rhymes"": [
      ""project"",
      ""subject"",
      ""aspect"",
      ""object"",
      ""reflect"",
      ""direct"",
      ""connect"",
      ""expect"",
      ""correct"",
      ""neglect"",
      ""suspect"",
      ""collect"",
      ""select"",
      ""protect"",
      ""predict"",
      ""reject"",
      ""detect"",
      ""insect"",
      ""addict"",
      ""inflict"",
      ""inject""
    ],
    ""soundsLike"": [
      ""protect"",
      ""project"",
      ""profit"",
      ""predict"",
      ""pact""
    ]
  },
  {
    ""word"": ""permit"",
    ""rhymes"": [
      ""fit"",
      ""split"",
      ""grit"",
      ""quit"",
      ""kit"",
      ""submit"",
      ""admit"",
      ""omit""
    ],
    ""soundsLike"": [
      ""promote"",
      ""peanut""
    ]
  },
  {
    ""word"": ""person"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""prison""
    ]
  },
  {
    ""word"": ""pet"",
    ""rhymes"": [
      ""net"",
      ""upset"",
      ""asset"",
      ""wet"",
      ""forget"",
      ""regret"",
      ""sunset""
    ],
    ""soundsLike"": [
      ""put""
    ]
  },
  {
    ""word"": ""phone"",
    ""rhymes"": [
      ""bone"",
      ""zone"",
      ""stone"",
      ""tone"",
      ""loan"",
      ""own"",
      ""alone"",
      ""ozone"",
      ""unknown""
    ],
    ""soundsLike"": [
      ""fun"",
      ""fine"",
      ""fan"",
      ""foam"",
      ""fame""
    ]
  },
  {
    ""word"": ""photo"",
    ""rhymes"": [
      ""know"",
      ""snow"",
      ""throw"",
      ""hello"",
      ""grow"",
      ""shadow"",
      ""potato"",
      ""toe"",
      ""glow"",
      ""slow"",
      ""tomato"",
      ""below"",
      ""radio"",
      ""meadow"",
      ""tomorrow"",
      ""borrow"",
      ""tornado"",
      ""buffalo""
    ],
    ""soundsLike"": [
      ""fat""
    ]
  },
  {
    ""word"": ""phrase"",
    ""rhymes"": [
      ""raise"",
      ""praise"",
      ""gaze"",
      ""always"",
      ""maze""
    ],
    ""soundsLike"": [
      ""face""
    ]
  },
  {
    ""word"": ""physical"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""fiscal"",
      ""vehicle"",
      ""bicycle""
    ]
  },
  {
    ""word"": ""piano"",
    ""rhymes"": [
      ""banana""
    ],
    ""soundsLike"": []
  },
  {
    ""word"": ""picnic"",
    ""rhymes"": [
      ""stick"",
      ""trick"",
      ""kick"",
      ""quick"",
      ""sick"",
      ""click"",
      ""brick""
    ],
    ""soundsLike"": [
      ""panic""
    ]
  },
  {
    ""word"": ""picture"",
    ""rhymes"": [],
    ""soundsLike"": []
  },
  {
    ""word"": ""piece"",
    ""rhymes"": [
      ""peace"",
      ""increase"",
      ""release"",
      ""nice"",
      ""police"",
      ""decrease""
    ],
    ""soundsLike"": [
      ""peace"",
      ""pass"",
      ""pause""
    ]
  },
  {
    ""word"": ""pig"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""bag""
    ]
  },
  {
    ""word"": ""pigeon"",
    ""rhymes"": [],
    ""soundsLike"": []
  },
  {
    ""word"": ""pill"",
    ""rhymes"": [
      ""will"",
      ""still"",
      ""drill"",
      ""hill"",
      ""skill"",
      ""ill"",
      ""until""
    ],
    ""soundsLike"": [
      ""pool"",
      ""pull"",
      ""pole"",
      ""pitch""
    ]
  },
  {
    ""word"": ""pilot"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""bullet""
    ]
  },
  {
    ""word"": ""pink"",
    ""rhymes"": [
      ""drink"",
      ""link"",
      ""wink""
    ],
    ""soundsLike"": [
      ""wink""
    ]
  },
  {
    ""word"": ""pioneer"",
    ""rhymes"": [
      ""year"",
      ""deer"",
      ""appear"",
      ""near"",
      ""sphere""
    ],
    ""soundsLike"": [
      ""piano"",
      ""panther""
    ]
  },
  {
    ""word"": ""pipe"",
    ""rhymes"": [
      ""type""
    ],
    ""soundsLike"": []
  },
  {
    ""word"": ""pistol"",
    ""rhymes"": [
      ""crystal""
    ],
    ""soundsLike"": [
      ""puzzle""
    ]
  },
  {
    ""word"": ""pitch"",
    ""rhymes"": [
      ""switch"",
      ""rich"",
      ""enrich""
    ],
    ""soundsLike"": [
      ""patch"",
      ""page"",
      ""push"",
      ""pill"",
      ""beach""
    ]
  },
  {
    ""word"": ""pizza"",
    ""rhymes"": [],
    ""soundsLike"": []
  },
  {
    ""word"": ""place"",
    ""rhymes"": [
      ""case"",
      ""grace"",
      ""face"",
      ""base"",
      ""space"",
      ""race"",
      ""embrace"",
      ""chase"",
      ""replace"",
      ""erase""
    ],
    ""soundsLike"": [
      ""play"",
      ""please"",
      ""replace"",
      ""bless"",
      ""blouse"",
      ""peace"",
      ""pass"",
      ""piece"",
      ""palace""
    ]
  },
  {
    ""word"": ""planet"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""peanut"",
      ""piano""
    ]
  },
  {
    ""word"": ""plastic"",
    ""rhymes"": [
      ""drastic""
    ],
    ""soundsLike"": [
      ""blast""
    ]
  },
  {
    ""word"": ""plate"",
    ""rhymes"": [
      ""state"",
      ""rate"",
      ""great"",
      ""mandate"",
      ""estate"",
      ""gate"",
      ""indicate"",
      ""update"",
      ""wait"",
      ""debate""
    ],
    ""soundsLike"": [
      ""play"",
      ""blade""
    ]
  },
  {
    ""word"": ""play"",
    ""rhymes"": [
      ""day"",
      ""away"",
      ""way"",
      ""say"",
      ""survey"",
      ""display"",
      ""stay"",
      ""delay"",
      ""holiday"",
      ""essay"",
      ""clay"",
      ""betray"",
      ""spray"",
      ""they"",
      ""okay"",
      ""tray"",
      ""obey"",
      ""today""
    ],
    ""soundsLike"": [
      ""place"",
      ""plate"",
      ""blue""
    ]
  },
  {
    ""word"": ""please"",
    ""rhymes"": [
      ""disease"",
      ""cheese"",
      ""squeeze"",
      ""breeze""
    ],
    ""soundsLike"": [
      ""place"",
      ""peace"",
      ""piece"",
      ""pause""
    ]
  },
  {
    ""word"": ""pledge"",
    ""rhymes"": [
      ""edge""
    ],
    ""soundsLike"": [
      ""plug"",
      ""plunge"",
      ""play"",
      ""page"",
      ""blush""
    ]
  },
  {
    ""word"": ""pluck"",
    ""rhymes"": [
      ""duck"",
      ""truck""
    ],
    ""soundsLike"": [
      ""plug"",
      ""black"",
      ""bleak""
    ]
  },
  {
    ""word"": ""plug"",
    ""rhymes"": [
      ""rug"",
      ""shrug""
    ],
    ""soundsLike"": [
      ""pluck"",
      ""pledge"",
      ""pig"",
      ""play""
    ]
  },
  {
    ""word"": ""plunge"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""pledge"",
      ""punch"",
      ""plug""
    ]
  },
  {
    ""word"": ""poem"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""palm""
    ]
  },
  {
    ""word"": ""poet"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""post"",
      ""pet"",
      ""parrot"",
      ""peanut"",
      ""pact""
    ]
  },
  {
    ""word"": ""point"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""pond""
    ]
  },
  {
    ""word"": ""polar"",
    ""rhymes"": [
      ""solar""
    ],
    ""soundsLike"": [
      ""pole"",
      ""pair""
    ]
  },
  {
    ""word"": ""pole"",
    ""rhymes"": [
      ""control"",
      ""hole"",
      ""soul"",
      ""enroll"",
      ""patrol""
    ],
    ""soundsLike"": [
      ""pool"",
      ""pull"",
      ""pill"",
      ""polar""
    ]
  },
  {
    ""word"": ""police"",
    ""rhymes"": [
      ""piece"",
      ""peace"",
      ""increase"",
      ""release"",
      ""nice"",
      ""decrease""
    ],
    ""soundsLike"": [
      ""palace"",
      ""pulse"",
      ""place"",
      ""please""
    ]
  },
  {
    ""word"": ""pond"",
    ""rhymes"": [
      ""beyond""
    ],
    ""soundsLike"": [
      ""point"",
      ""bind"",
      ""palm""
    ]
  },
  {
    ""word"": ""pony"",
    ""rhymes"": [
      ""tree"",
      ""sea"",
      ""key"",
      ""flee"",
      ""degree"",
      ""debris"",
      ""agree"",
      ""fee"",
      ""ski"",
      ""three"",
      ""knee""
    ],
    ""soundsLike"": []
  },
  {
    ""word"": ""pool"",
    ""rhymes"": [
      ""school"",
      ""rule"",
      ""cool"",
      ""tool"",
      ""fuel"",
      ""stool"",
      ""cruel"",
      ""mule"",
      ""jewel""
    ],
    ""soundsLike"": [
      ""pull"",
      ""pole"",
      ""pill"",
      ""ball""
    ]
  },
  {
    ""word"": ""popular"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""people""
    ]
  },
  {
    ""word"": ""portion"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""person""
    ]
  },
  {
    ""word"": ""position"",
    ""rhymes"": [
      ""tuition""
    ],
    ""soundsLike"": [
      ""portion"",
      ""pigeon""
    ]
  },
  {
    ""word"": ""possible"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""pistol""
    ]
  },
  {
    ""word"": ""post"",
    ""rhymes"": [
      ""host"",
      ""coast"",
      ""roast"",
      ""ghost"",
      ""toast"",
      ""almost""
    ],
    ""soundsLike"": [
      ""poet"",
      ""best"",
      ""host"",
      ""boost"",
      ""pass""
    ]
  },
  {
    ""word"": ""potato"",
    ""rhymes"": [
      ""know"",
      ""snow"",
      ""throw"",
      ""hello"",
      ""grow"",
      ""shadow"",
      ""toe"",
      ""glow"",
      ""slow"",
      ""tomato"",
      ""below"",
      ""radio"",
      ""meadow"",
      ""tomorrow"",
      ""borrow"",
      ""photo"",
      ""tornado"",
      ""buffalo""
    ],
    ""soundsLike"": []
  },
  {
    ""word"": ""pottery"",
    ""rhymes"": [
      ""lottery""
    ],
    ""soundsLike"": [
      ""pattern""
    ]
  },
  {
    ""word"": ""poverty"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""party""
    ]
  },
  {
    ""word"": ""powder"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""power""
    ]
  },
  {
    ""word"": ""power"",
    ""rhymes"": [
      ""flower"",
      ""hour"",
      ""empower"",
      ""tower""
    ],
    ""soundsLike"": [
      ""pair"",
      ""pear"",
      ""bar"",
      ""powder""
    ]
  },
  {
    ""word"": ""practice"",
    ""rhymes"": [
      ""cactus""
    ],
    ""soundsLike"": [
      ""produce""
    ]
  },
  {
    ""word"": ""praise"",
    ""rhymes"": [
      ""raise"",
      ""phrase"",
      ""gaze"",
      ""always"",
      ""maze""
    ],
    ""soundsLike"": [
      ""prize"",
      ""price"",
      ""breeze""
    ]
  },
  {
    ""word"": ""predict"",
    ""rhymes"": [
      ""perfect"",
      ""addict"",
      ""inflict""
    ],
    ""soundsLike"": [
      ""protect"",
      ""perfect"",
      ""project""
    ]
  },
  {
    ""word"": ""prefer"",
    ""rhymes"": [
      ""transfer"",
      ""occur"",
      ""amateur"",
      ""blur""
    ],
    ""soundsLike"": [
      ""proof"",
      ""brother""
    ]
  },
  {
    ""word"": ""prepare"",
    ""rhymes"": [
      ""air"",
      ""chair"",
      ""wear"",
      ""square"",
      ""share"",
      ""despair"",
      ""there"",
      ""hair"",
      ""spare"",
      ""aware"",
      ""pair"",
      ""where"",
      ""rare"",
      ""swear"",
      ""pear"",
      ""affair"",
      ""repair"",
      ""glare"",
      ""scare"",
      ""warfare"",
      ""unfair"",
      ""unaware""
    ],
    ""soundsLike"": [
      ""paper""
    ]
  },
  {
    ""word"": ""present"",
    ""rhymes"": [
      ""rent"",
      ""prevent"",
      ""segment"",
      ""tent"",
      ""orient"",
      ""cement"",
      ""peasant"",
      ""frequent""
    ],
    ""soundsLike"": [
      ""peasant"",
      ""prison"",
      ""parent"",
      ""prevent"",
      ""print""
    ]
  },
  {
    ""word"": ""pretty"",
    ""rhymes"": [
      ""city""
    ],
    ""soundsLike"": []
  },
  {
    ""word"": ""prevent"",
    ""rhymes"": [
      ""present"",
      ""rent"",
      ""segment"",
      ""tent"",
      ""orient"",
      ""cement"",
      ""frequent""
    ],
    ""soundsLike"": [
      ""private"",
      ""present"",
      ""print"",
      ""provide""
    ]
  },
  {
    ""word"": ""price"",
    ""rhymes"": [
      ""ice"",
      ""advice"",
      ""device"",
      ""nice"",
      ""rice"",
      ""dice"",
      ""spice"",
      ""slice"",
      ""twice""
    ],
    ""soundsLike"": [
      ""prize"",
      ""praise"",
      ""purse"",
      ""brass""
    ]
  },
  {
    ""word"": ""pride"",
    ""rhymes"": [
      ""side"",
      ""provide"",
      ""ride"",
      ""slide"",
      ""tide"",
      ""guide"",
      ""decide"",
      ""divide"",
      ""wide"",
      ""glide"",
      ""inside"",
      ""outside""
    ],
    ""soundsLike"": [
      ""proud"",
      ""parade"",
      ""bright"",
      ""bread""
    ]
  },
  {
    ""word"": ""primary"",
    ""rhymes"": [
      ""cherry"",
      ""carry"",
      ""library"",
      ""very"",
      ""ordinary"",
      ""merry"",
      ""february""
    ],
    ""soundsLike"": []
  },
  {
    ""word"": ""print"",
    ""rhymes"": [
      ""hint""
    ],
    ""soundsLike"": [
      ""present"",
      ""parent"",
      ""point""
    ]
  },
  {
    ""word"": ""priority"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""purity"",
      ""property"",
      ""private""
    ]
  },
  {
    ""word"": ""prison"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""present"",
      ""person""
    ]
  },
  {
    ""word"": ""private"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""profit"",
      ""provide"",
      ""prevent"",
      ""permit""
    ]
  },
  {
    ""word"": ""prize"",
    ""rhymes"": [
      ""exercise"",
      ""demise"",
      ""wise"",
      ""size"",
      ""surprise""
    ],
    ""soundsLike"": [
      ""price"",
      ""praise"",
      ""breeze""
    ]
  },
  {
    ""word"": ""problem"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""palm""
    ]
  },
  {
    ""word"": ""process"",
    ""rhymes"": [
      ""address"",
      ""access"",
      ""express"",
      ""success"",
      ""dress"",
      ""guess"",
      ""excess"",
      ""bless""
    ],
    ""soundsLike"": []
  },
  {
    ""word"": ""produce"",
    ""rhymes"": [
      ""use"",
      ""abuse"",
      ""goose"",
      ""juice"",
      ""reduce"",
      ""excuse""
    ],
    ""soundsLike"": [
      ""reduce""
    ]
  },
  {
    ""word"": ""profit"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""private"",
      ""perfect"",
      ""permit""
    ]
  },
  {
    ""word"": ""program"",
    ""rhymes"": [
      ""slam"",
      ""diagram"",
      ""cram""
    ],
    ""soundsLike"": []
  },
  {
    ""word"": ""project"",
    ""rhymes"": [
      ""subject"",
      ""aspect"",
      ""object"",
      ""perfect"",
      ""reflect"",
      ""direct"",
      ""connect"",
      ""expect"",
      ""correct"",
      ""neglect"",
      ""suspect"",
      ""collect"",
      ""select"",
      ""protect"",
      ""reject"",
      ""detect"",
      ""insect"",
      ""inject""
    ],
    ""soundsLike"": [
      ""perfect"",
      ""protect"",
      ""predict"",
      ""reject"",
      ""profit""
    ]
  },
  {
    ""word"": ""promote"",
    ""rhymes"": [
      ""note"",
      ""boat"",
      ""float"",
      ""quote"",
      ""goat"",
      ""vote"",
      ""devote""
    ],
    ""soundsLike"": [
      ""permit"",
      ""print""
    ]
  },
  {
    ""word"": ""proof"",
    ""rhymes"": [
      ""roof""
    ],
    ""soundsLike"": [
      ""brief"",
      ""prefer""
    ]
  },
  {
    ""word"": ""property"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""party"",
      ""pretty""
    ]
  },
  {
    ""word"": ""prosper"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""paper"",
      ""pepper""
    ]
  },
  {
    ""word"": ""protect"",
    ""rhymes"": [
      ""project"",
      ""subject"",
      ""aspect"",
      ""object"",
      ""perfect"",
      ""reflect"",
      ""direct"",
      ""connect"",
      ""expect"",
      ""correct"",
      ""neglect"",
      ""suspect"",
      ""collect"",
      ""select"",
      ""reject"",
      ""detect"",
      ""insect"",
      ""inject""
    ],
    ""soundsLike"": [
      ""predict"",
      ""project"",
      ""perfect""
    ]
  },
  {
    ""word"": ""proud"",
    ""rhymes"": [
      ""cloud"",
      ""crowd"",
      ""loud""
    ],
    ""soundsLike"": [
      ""pride"",
      ""parade"",
      ""bread""
    ]
  },
  {
    ""word"": ""provide"",
    ""rhymes"": [
      ""side"",
      ""ride"",
      ""slide"",
      ""pride"",
      ""tide"",
      ""guide"",
      ""decide"",
      ""divide"",
      ""wide"",
      ""glide"",
      ""inside"",
      ""outside""
    ],
    ""soundsLike"": [
      ""private"",
      ""pride"",
      ""prevent"",
      ""parade""
    ]
  },
  {
    ""word"": ""public"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""pluck""
    ]
  },
  {
    ""word"": ""pudding"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""paddle""
    ]
  },
  {
    ""word"": ""pull"",
    ""rhymes"": [
      ""wool""
    ],
    ""soundsLike"": [
      ""pool"",
      ""pole"",
      ""pill"",
      ""push"",
      ""ball""
    ]
  },
  {
    ""word"": ""pulp"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""bulb"",
      ""pill"",
      ""help""
    ]
  },
  {
    ""word"": ""pulse"",
    ""rhymes"": [
      ""impulse""
    ],
    ""soundsLike"": [
      ""police"",
      ""pill""
    ]
  },
  {
    ""word"": ""pumpkin"",
    ""rhymes"": [],
    ""soundsLike"": []
  },
  {
    ""word"": ""punch"",
    ""rhymes"": [
      ""crunch"",
      ""lunch""
    ],
    ""soundsLike"": [
      ""bench"",
      ""pen"",
      ""pitch"",
      ""pony""
    ]
  },
  {
    ""word"": ""pupil"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""people""
    ]
  },
  {
    ""word"": ""puppy"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""pepper""
    ]
  },
  {
    ""word"": ""purchase"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""purpose"",
      ""price"",
      ""palace""
    ]
  },
  {
    ""word"": ""purity"",
    ""rhymes"": [
      ""security""
    ],
    ""soundsLike"": [
      ""pretty"",
      ""party"",
      ""priority"",
      ""parrot""
    ]
  },
  {
    ""word"": ""purpose"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""price"",
      ""purchase""
    ]
  },
  {
    ""word"": ""purse"",
    ""rhymes"": [
      ""nurse"",
      ""universe""
    ],
    ""soundsLike"": [
      ""peace"",
      ""pass"",
      ""piece"",
      ""price""
    ]
  },
  {
    ""word"": ""push"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""page"",
      ""pull"",
      ""put"",
      ""pitch"",
      ""patch""
    ]
  },
  {
    ""word"": ""put"",
    ""rhymes"": [
      ""foot"",
      ""input"",
      ""output""
    ],
    ""soundsLike"": [
      ""pet"",
      ""push""
    ]
  },
  {
    ""word"": ""puzzle"",
    ""rhymes"": [],
    ""soundsLike"": []
  },
  {
    ""word"": ""pyramid"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""promote"",
      ""parent"",
      ""permit""
    ]
  },
  {
    ""word"": ""quality"",
    ""rhymes"": [],
    ""soundsLike"": []
  },
  {
    ""word"": ""quantum"",
    ""rhymes"": [],
    ""soundsLike"": []
  },
  {
    ""word"": ""quarter"",
    ""rhymes"": [],
    ""soundsLike"": []
  },
  {
    ""word"": ""question"",
    ""rhymes"": [
      ""session""
    ],
    ""soundsLike"": [
      ""kitchen""
    ]
  },
  {
    ""word"": ""quick"",
    ""rhymes"": [
      ""stick"",
      ""trick"",
      ""kick"",
      ""sick"",
      ""click"",
      ""brick"",
      ""picnic""
    ],
    ""soundsLike"": [
      ""kick"",
      ""click"",
      ""cake""
    ]
  },
  {
    ""word"": ""quit"",
    ""rhymes"": [
      ""fit"",
      ""split"",
      ""grit"",
      ""kit"",
      ""submit"",
      ""admit"",
      ""permit"",
      ""omit""
    ],
    ""soundsLike"": [
      ""quote"",
      ""cute"",
      ""kit"",
      ""quiz"",
      ""coyote""
    ]
  },
  {
    ""word"": ""quiz"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""quit"",
      ""close"",
      ""cause""
    ]
  },
  {
    ""word"": ""quote"",
    ""rhymes"": [
      ""note"",
      ""boat"",
      ""promote"",
      ""float"",
      ""goat"",
      ""vote"",
      ""devote""
    ],
    ""soundsLike"": [
      ""quit"",
      ""coyote"",
      ""cute"",
      ""cat"",
      ""caught"",
      ""code""
    ]
  },
  {
    ""word"": ""rabbit"",
    ""rhymes"": [
      ""habit""
    ],
    ""soundsLike"": [
      ""robot"",
      ""rapid"",
      ""habit"",
      ""repeat"",
      ""riot"",
      ""robust""
    ]
  },
  {
    ""word"": ""raccoon"",
    ""rhymes"": [
      ""moon"",
      ""spoon"",
      ""soon"",
      ""immune"",
      ""dune""
    ],
    ""soundsLike"": []
  },
  {
    ""word"": ""race"",
    ""rhymes"": [
      ""case"",
      ""grace"",
      ""face"",
      ""base"",
      ""place"",
      ""space"",
      ""embrace"",
      ""chase"",
      ""replace"",
      ""erase""
    ],
    ""soundsLike"": [
      ""rice"",
      ""raise"",
      ""grace"",
      ""erase""
    ]
  },
  {
    ""word"": ""rack"",
    ""rhymes"": [
      ""black"",
      ""track"",
      ""attack"",
      ""crack"",
      ""snack""
    ],
    ""soundsLike"": [
      ""wreck"",
      ""crack""
    ]
  },
  {
    ""word"": ""radar"",
    ""rhymes"": [
      ""bar"",
      ""car"",
      ""jar"",
      ""guitar"",
      ""seminar"",
      ""jaguar"",
      ""cigar""
    ],
    ""soundsLike"": [
      ""retire"",
      ""radio""
    ]
  },
  {
    ""word"": ""radio"",
    ""rhymes"": [
      ""know"",
      ""snow"",
      ""throw"",
      ""hello"",
      ""grow"",
      ""shadow"",
      ""potato"",
      ""toe"",
      ""glow"",
      ""slow"",
      ""tomato"",
      ""below"",
      ""meadow"",
      ""tomorrow"",
      ""borrow"",
      ""photo"",
      ""tornado"",
      ""buffalo""
    ],
    ""soundsLike"": [
      ""ready""
    ]
  },
  {
    ""word"": ""rail"",
    ""rhymes"": [
      ""scale"",
      ""mail"",
      ""whale"",
      ""detail"",
      ""tail"",
      ""sail"",
      ""female"",
      ""unveil"",
      ""inhale""
    ],
    ""soundsLike"": [
      ""real"",
      ""rule""
    ]
  },
  {
    ""word"": ""rain"",
    ""rhymes"": [
      ""train"",
      ""domain"",
      ""sustain"",
      ""again"",
      ""grain"",
      ""obtain"",
      ""brain"",
      ""gain"",
      ""explain"",
      ""main"",
      ""crane"",
      ""insane"",
      ""remain""
    ],
    ""soundsLike"": [
      ""around"",
      ""run"",
      ""brain"",
      ""earn"",
      ""grain"",
      ""crane"",
      ""arrange""
    ]
  },
  {
    ""word"": ""raise"",
    ""rhymes"": [
      ""phrase"",
      ""praise"",
      ""gaze"",
      ""always"",
      ""maze""
    ],
    ""soundsLike"": [
      ""rose"",
      ""race""
    ]
  },
  {
    ""word"": ""rally"",
    ""rhymes"": [
      ""valley"",
      ""alley""
    ],
    ""soundsLike"": [
      ""early"",
      ""rely""
    ]
  },
  {
    ""word"": ""ramp"",
    ""rhymes"": [
      ""camp"",
      ""lamp"",
      ""stamp"",
      ""damp""
    ],
    ""soundsLike"": [
      ""wrap""
    ]
  },
  {
    ""word"": ""ranch"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""range"",
      ""arrange"",
      ""around"",
      ""rain"",
      ""crunch"",
      ""arena""
    ]
  },
  {
    ""word"": ""random"",
    ""rhymes"": [],
    ""soundsLike"": []
  },
  {
    ""word"": ""range"",
    ""rhymes"": [
      ""change"",
      ""exchange"",
      ""arrange""
    ],
    ""soundsLike"": [
      ""arrange"",
      ""rain"",
      ""ranch"",
      ""orange"",
      ""around"",
      ""arena"",
      ""fringe""
    ]
  },
  {
    ""word"": ""rapid"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""rabbit"",
      ""repeat"",
      ""robot""
    ]
  },
  {
    ""word"": ""rare"",
    ""rhymes"": [
      ""air"",
      ""chair"",
      ""wear"",
      ""square"",
      ""share"",
      ""despair"",
      ""there"",
      ""hair"",
      ""spare"",
      ""aware"",
      ""pair"",
      ""where"",
      ""swear"",
      ""pear"",
      ""affair"",
      ""repair"",
      ""prepare"",
      ""glare"",
      ""scare"",
      ""warfare"",
      ""unfair"",
      ""unaware""
    ],
    ""soundsLike"": [
      ""hair""
    ]
  },
  {
    ""word"": ""rate"",
    ""rhymes"": [
      ""state"",
      ""plate"",
      ""great"",
      ""mandate"",
      ""estate"",
      ""gate"",
      ""indicate"",
      ""update"",
      ""wait"",
      ""debate""
    ],
    ""soundsLike"": [
      ""right"",
      ""write"",
      ""route"",
      ""great""
    ]
  },
  {
    ""word"": ""rather"",
    ""rhymes"": [
      ""other"",
      ""another"",
      ""mother"",
      ""gather"",
      ""brother""
    ],
    ""soundsLike"": [
      ""brother""
    ]
  },
  {
    ""word"": ""raven"",
    ""rhymes"": [],
    ""soundsLike"": []
  },
  {
    ""word"": ""raw"",
    ""rhymes"": [
      ""law"",
      ""draw"",
      ""claw""
    ],
    ""soundsLike"": []
  },
  {
    ""word"": ""razor"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""raise"",
      ""race""
    ]
  },
  {
    ""word"": ""ready"",
    ""rhymes"": [
      ""already""
    ],
    ""soundsLike"": [
      ""already""
    ]
  },
  {
    ""word"": ""real"",
    ""rhymes"": [
      ""deal"",
      ""wheel"",
      ""feel"",
      ""steel"",
      ""reveal""
    ],
    ""soundsLike"": [
      ""rule"",
      ""rail""
    ]
  },
  {
    ""word"": ""reason"",
    ""rhymes"": [
      ""season""
    ],
    ""soundsLike"": [
      ""prison"",
      ""frozen""
    ]
  },
  {
    ""word"": ""rebel"",
    ""rhymes"": [
      ""spell"",
      ""shell"",
      ""tell"",
      ""sell"",
      ""hotel""
    ],
    ""soundsLike"": [
      ""ripple"",
      ""trouble""
    ]
  },
  {
    ""word"": ""rebuild"",
    ""rhymes"": [
      ""build""
    ],
    ""soundsLike"": [
      ""rebel"",
      ""rabbit"",
      ""rapid""
    ]
  },
  {
    ""word"": ""recall"",
    ""rhymes"": [
      ""all"",
      ""ball"",
      ""call"",
      ""fall"",
      ""wall"",
      ""small"",
      ""alcohol"",
      ""doll"",
      ""install"",
      ""crawl""
    ],
    ""soundsLike"": []
  },
  {
    ""word"": ""receive"",
    ""rhymes"": [
      ""leave"",
      ""naive"",
      ""believe"",
      ""achieve""
    ],
    ""soundsLike"": []
  },
  {
    ""word"": ""recipe"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""wrestle""
    ]
  },
  {
    ""word"": ""record"",
    ""rhymes"": [
      ""board"",
      ""afford"",
      ""toward"",
      ""reward"",
      ""sword""
    ],
    ""soundsLike"": [
      ""reward""
    ]
  },
  {
    ""word"": ""recycle"",
    ""rhymes"": [
      ""cycle""
    ],
    ""soundsLike"": [
      ""recall"",
      ""wrestle""
    ]
  },
  {
    ""word"": ""reduce"",
    ""rhymes"": [
      ""use"",
      ""abuse"",
      ""produce"",
      ""goose"",
      ""juice"",
      ""excuse""
    ],
    ""soundsLike"": [
      ""produce"",
      ""ready""
    ]
  },
  {
    ""word"": ""reflect"",
    ""rhymes"": [
      ""project"",
      ""subject"",
      ""aspect"",
      ""object"",
      ""perfect"",
      ""direct"",
      ""connect"",
      ""expect"",
      ""correct"",
      ""neglect"",
      ""suspect"",
      ""collect"",
      ""select"",
      ""protect"",
      ""reject"",
      ""detect"",
      ""insect"",
      ""inject""
    ],
    ""soundsLike"": [
      ""inflict"",
      ""collect"",
      ""select"",
      ""reject"",
      ""perfect""
    ]
  },
  {
    ""word"": ""reform"",
    ""rhymes"": [
      ""inform"",
      ""warm"",
      ""uniform"",
      ""swarm""
    ],
    ""soundsLike"": [
      ""inform""
    ]
  },
  {
    ""word"": ""refuse"",
    ""rhymes"": [
      ""choose"",
      ""abuse"",
      ""news"",
      ""cruise"",
      ""excuse"",
      ""accuse""
    ],
    ""soundsLike"": [
      ""review""
    ]
  },
  {
    ""word"": ""region"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""reason""
    ]
  },
  {
    ""word"": ""regret"",
    ""rhymes"": [
      ""net"",
      ""upset"",
      ""asset"",
      ""wet"",
      ""forget"",
      ""pet"",
      ""sunset""
    ],
    ""soundsLike"": [
      ""record""
    ]
  },
  {
    ""word"": ""regular"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""require""
    ]
  },
  {
    ""word"": ""reject"",
    ""rhymes"": [
      ""project"",
      ""subject"",
      ""aspect"",
      ""object"",
      ""perfect"",
      ""reflect"",
      ""direct"",
      ""connect"",
      ""expect"",
      ""correct"",
      ""neglect"",
      ""suspect"",
      ""collect"",
      ""select"",
      ""protect"",
      ""detect"",
      ""insect"",
      ""inject""
    ],
    ""soundsLike"": [
      ""project"",
      ""rigid"",
      ""inject"",
      ""reflect""
    ]
  },
  {
    ""word"": ""relax"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""release""
    ]
  },
  {
    ""word"": ""release"",
    ""rhymes"": [
      ""piece"",
      ""peace"",
      ""increase"",
      ""nice"",
      ""police"",
      ""decrease""
    ],
    ""soundsLike"": [
      ""relax"",
      ""relief"",
      ""replace"",
      ""rally"",
      ""rely""
    ]
  },
  {
    ""word"": ""relief"",
    ""rhymes"": [
      ""leaf"",
      ""brief"",
      ""chief"",
      ""grief"",
      ""beef""
    ],
    ""soundsLike"": [
      ""rally"",
      ""rely"",
      ""early""
    ]
  },
  {
    ""word"": ""rely"",
    ""rhymes"": [
      ""fly"",
      ""eye"",
      ""high"",
      ""supply"",
      ""identify"",
      ""dry"",
      ""shy"",
      ""cry"",
      ""clarify"",
      ""try"",
      ""verify"",
      ""deny"",
      ""defy"",
      ""modify"",
      ""satisfy"",
      ""spy""
    ],
    ""soundsLike"": [
      ""rally""
    ]
  },
  {
    ""word"": ""remain"",
    ""rhymes"": [
      ""train"",
      ""domain"",
      ""sustain"",
      ""rain"",
      ""again"",
      ""grain"",
      ""obtain"",
      ""brain"",
      ""gain"",
      ""explain"",
      ""main"",
      ""crane"",
      ""insane""
    ],
    ""soundsLike"": []
  },
  {
    ""word"": ""remember"",
    ""rhymes"": [
      ""member"",
      ""december""
    ],
    ""soundsLike"": [
      ""remain""
    ]
  },
  {
    ""word"": ""remind"",
    ""rhymes"": [
      ""bind"",
      ""mind"",
      ""find"",
      ""kind"",
      ""blind"",
      ""behind""
    ],
    ""soundsLike"": [
      ""remain""
    ]
  },
  {
    ""word"": ""remove"",
    ""rhymes"": [
      ""move"",
      ""improve"",
      ""approve""
    ],
    ""soundsLike"": [
      ""receive"",
      ""arena"",
      ""renew""
    ]
  },
  {
    ""word"": ""render"",
    ""rhymes"": [
      ""vendor"",
      ""slender""
    ],
    ""soundsLike"": [
      ""around"",
      ""rent""
    ]
  },
  {
    ""word"": ""renew"",
    ""rhymes"": [
      ""blue"",
      ""into"",
      ""you"",
      ""view"",
      ""review"",
      ""true"",
      ""two"",
      ""shoe"",
      ""crew"",
      ""tissue"",
      ""few"",
      ""glue"",
      ""bamboo"",
      ""undo"",
      ""tattoo"",
      ""canoe"",
      ""zoo""
    ],
    ""soundsLike"": [
      ""arena"",
      ""rent""
    ]
  },
  {
    ""word"": ""rent"",
    ""rhymes"": [
      ""present"",
      ""prevent"",
      ""segment"",
      ""tent"",
      ""orient"",
      ""cement"",
      ""frequent""
    ],
    ""soundsLike"": [
      ""around"",
      ""round"",
      ""front"",
      ""current""
    ]
  },
  {
    ""word"": ""reopen"",
    ""rhymes"": [
      ""open""
    ],
    ""soundsLike"": [
      ""reason"",
      ""ribbon"",
      ""region""
    ]
  },
  {
    ""word"": ""repair"",
    ""rhymes"": [
      ""air"",
      ""chair"",
      ""wear"",
      ""square"",
      ""share"",
      ""despair"",
      ""there"",
      ""hair"",
      ""spare"",
      ""aware"",
      ""pair"",
      ""where"",
      ""rare"",
      ""swear"",
      ""pear"",
      ""affair"",
      ""prepare"",
      ""glare"",
      ""scare"",
      ""warfare"",
      ""unfair"",
      ""unaware""
    ],
    ""soundsLike"": [
      ""report"",
      ""prepare"",
      ""rubber"",
      ""ripple""
    ]
  },
  {
    ""word"": ""repeat"",
    ""rhymes"": [
      ""seat"",
      ""treat"",
      ""street"",
      ""elite"",
      ""sweet"",
      ""meat"",
      ""retreat"",
      ""athlete"",
      ""wheat""
    ],
    ""soundsLike"": [
      ""rabbit"",
      ""robot"",
      ""report"",
      ""rapid""
    ]
  },
  {
    ""word"": ""replace"",
    ""rhymes"": [
      ""case"",
      ""grace"",
      ""face"",
      ""base"",
      ""place"",
      ""space"",
      ""race"",
      ""embrace"",
      ""chase"",
      ""erase""
    ],
    ""soundsLike"": [
      ""release""
    ]
  },
  {
    ""word"": ""report"",
    ""rhymes"": [
      ""short"",
      ""sport"",
      ""sort"",
      ""airport""
    ],
    ""soundsLike"": [
      ""repeat"",
      ""reward"",
      ""repair"",
      ""airport""
    ]
  },
  {
    ""word"": ""require"",
    ""rhymes"": [
      ""fire"",
      ""wire"",
      ""inspire"",
      ""acquire"",
      ""hire"",
      ""entire"",
      ""liar"",
      ""retire"",
      ""expire"",
      ""buyer""
    ],
    ""soundsLike"": [
      ""retire"",
      ""acquire""
    ]
  },
  {
    ""word"": ""rescue"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""risk""
    ]
  },
  {
    ""word"": ""resemble"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""rebel""
    ]
  },
  {
    ""word"": ""resist"",
    ""rhymes"": [
      ""list"",
      ""just"",
      ""twist"",
      ""assist"",
      ""exist"",
      ""enlist"",
      ""wrist""
    ],
    ""soundsLike"": [
      ""wrist"",
      ""exist"",
      ""arrest""
    ]
  },
  {
    ""word"": ""resource"",
    ""rhymes"": [],
    ""soundsLike"": []
  },
  {
    ""word"": ""response"",
    ""rhymes"": [],
    ""soundsLike"": []
  },
  {
    ""word"": ""result"",
    ""rhymes"": [
      ""adult""
    ],
    ""soundsLike"": [
      ""wrestle""
    ]
  },
  {
    ""word"": ""retire"",
    ""rhymes"": [
      ""fire"",
      ""wire"",
      ""inspire"",
      ""acquire"",
      ""require"",
      ""hire"",
      ""entire"",
      ""liar"",
      ""expire"",
      ""buyer""
    ],
    ""soundsLike"": [
      ""radar""
    ]
  },
  {
    ""word"": ""retreat"",
    ""rhymes"": [
      ""seat"",
      ""treat"",
      ""street"",
      ""elite"",
      ""sweet"",
      ""meat"",
      ""athlete"",
      ""wheat"",
      ""repeat""
    ],
    ""soundsLike"": [
      ""return"",
      ""rotate""
    ]
  },
  {
    ""word"": ""return"",
    ""rhymes"": [
      ""turn"",
      ""learn"",
      ""churn"",
      ""earn""
    ],
    ""soundsLike"": []
  },
  {
    ""word"": ""reunion"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""opinion"",
      ""region"",
      ""arena""
    ]
  },
  {
    ""word"": ""reveal"",
    ""rhymes"": [
      ""deal"",
      ""wheel"",
      ""feel"",
      ""steel"",
      ""real""
    ],
    ""soundsLike"": [
      ""rival""
    ]
  },
  {
    ""word"": ""review"",
    ""rhymes"": [
      ""blue"",
      ""into"",
      ""you"",
      ""view"",
      ""true"",
      ""two"",
      ""shoe"",
      ""crew"",
      ""tissue"",
      ""few"",
      ""glue"",
      ""bamboo"",
      ""undo"",
      ""tattoo"",
      ""renew"",
      ""canoe"",
      ""zoo""
    ],
    ""soundsLike"": [
      ""reveal""
    ]
  },
  {
    ""word"": ""reward"",
    ""rhymes"": [
      ""board"",
      ""record"",
      ""afford"",
      ""toward"",
      ""sword""
    ],
    ""soundsLike"": [
      ""record"",
      ""report"",
      ""toward""
    ]
  },
  {
    ""word"": ""rhythm"",
    ""rhymes"": [],
    ""soundsLike"": []
  },
  {
    ""word"": ""rib"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""wrap""
    ]
  },
  {
    ""word"": ""ribbon"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""urban""
    ]
  },
  {
    ""word"": ""rice"",
    ""rhymes"": [
      ""ice"",
      ""advice"",
      ""device"",
      ""nice"",
      ""price"",
      ""dice"",
      ""spice"",
      ""slice"",
      ""twice""
    ],
    ""soundsLike"": [
      ""race"",
      ""price"",
      ""raise""
    ]
  },
  {
    ""word"": ""rich"",
    ""rhymes"": [
      ""pitch"",
      ""switch"",
      ""enrich""
    ],
    ""soundsLike"": [
      ""ridge""
    ]
  },
  {
    ""word"": ""ride"",
    ""rhymes"": [
      ""side"",
      ""provide"",
      ""slide"",
      ""pride"",
      ""tide"",
      ""guide"",
      ""decide"",
      ""divide"",
      ""wide"",
      ""glide"",
      ""inside"",
      ""outside""
    ],
    ""soundsLike"": [
      ""right"",
      ""road"",
      ""write"",
      ""rude"",
      ""pride""
    ]
  },
  {
    ""word"": ""ridge"",
    ""rhymes"": [
      ""bridge""
    ],
    ""soundsLike"": [
      ""rich"",
      ""ring"",
      ""bridge"",
      ""urge"",
      ""rug""
    ]
  },
  {
    ""word"": ""rifle"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""rival"",
      ""royal"",
      ""shuffle""
    ]
  },
  {
    ""word"": ""right"",
    ""rhymes"": [
      ""light"",
      ""night"",
      ""write"",
      ""sight"",
      ""flight"",
      ""bright"",
      ""slight"",
      ""height"",
      ""kite"",
      ""invite"",
      ""excite"",
      ""midnight""
    ],
    ""soundsLike"": [
      ""write"",
      ""ride"",
      ""rate"",
      ""route"",
      ""bright""
    ]
  },
  {
    ""word"": ""rigid"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""reject"",
      ""rapid""
    ]
  },
  {
    ""word"": ""ring"",
    ""rhymes"": [
      ""bring"",
      ""spring"",
      ""swing"",
      ""sting"",
      ""thing"",
      ""wing"",
      ""sing""
    ],
    ""soundsLike"": [
      ""wrong"",
      ""run"",
      ""around"",
      ""rain"",
      ""bring"",
      ""during""
    ]
  },
  {
    ""word"": ""riot"",
    ""rhymes"": [
      ""diet""
    ],
    ""soundsLike"": [
      ""right"",
      ""write"",
      ""rate"",
      ""rabbit"",
      ""arrest""
    ]
  },
  {
    ""word"": ""ripple"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""rebel"",
      ""royal""
    ]
  },
  {
    ""word"": ""risk"",
    ""rhymes"": [
      ""brisk""
    ],
    ""soundsLike"": [
      ""brisk"",
      ""wreck"",
      ""race""
    ]
  },
  {
    ""word"": ""ritual"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""original"",
      ""virtual"",
      ""usual"",
      ""wrestle""
    ]
  },
  {
    ""word"": ""rival"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""reveal"",
      ""rifle"",
      ""travel"",
      ""royal""
    ]
  },
  {
    ""word"": ""river"",
    ""rhymes"": [
      ""deliver"",
      ""shiver""
    ],
    ""soundsLike"": [
      ""shiver"",
      ""rare"",
      ""arrive"",
      ""hover""
    ]
  },
  {
    ""word"": ""road"",
    ""rhymes"": [
      ""code"",
      ""load"",
      ""episode"",
      ""erode""
    ],
    ""soundsLike"": [
      ""ride"",
      ""rude"",
      ""rate"",
      ""erode""
    ]
  },
  {
    ""word"": ""roast"",
    ""rhymes"": [
      ""post"",
      ""host"",
      ""coast"",
      ""ghost"",
      ""toast"",
      ""almost""
    ],
    ""soundsLike"": [
      ""wrist"",
      ""arrest"",
      ""host""
    ]
  },
  {
    ""word"": ""robot"",
    ""rhymes"": [
      ""spot"",
      ""that"",
      ""thought"",
      ""nut"",
      ""what"",
      ""slot"",
      ""walnut"",
      ""caught"",
      ""peanut"",
      ""coconut""
    ],
    ""soundsLike"": [
      ""rabbit"",
      ""repeat"",
      ""robust"",
      ""rapid""
    ]
  },
  {
    ""word"": ""robust"",
    ""rhymes"": [
      ""trust"",
      ""just"",
      ""dust"",
      ""must"",
      ""adjust""
    ],
    ""soundsLike"": [
      ""robot"",
      ""rabbit"",
      ""roast"",
      ""wrist"",
      ""resist"",
      ""arrest""
    ]
  },
  {
    ""word"": ""rocket"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""riot"",
      ""cricket""
    ]
  },
  {
    ""word"": ""romance"",
    ""rhymes"": [
      ""enhance"",
      ""dance"",
      ""advance"",
      ""glance""
    ],
    ""soundsLike"": [
      ""remain""
    ]
  },
  {
    ""word"": ""roof"",
    ""rhymes"": [
      ""proof""
    ],
    ""soundsLike"": [
      ""rough"",
      ""proof"",
      ""arrive""
    ]
  },
  {
    ""word"": ""rookie"",
    ""rhymes"": [],
    ""soundsLike"": []
  },
  {
    ""word"": ""room"",
    ""rhymes"": [
      ""assume"",
      ""broom"",
      ""gloom""
    ],
    ""soundsLike"": [
      ""around"",
      ""rain"",
      ""broom""
    ]
  },
  {
    ""word"": ""rose"",
    ""rhymes"": [
      ""close"",
      ""nose"",
      ""impose"",
      ""expose"",
      ""oppose""
    ],
    ""soundsLike"": [
      ""raise""
    ]
  },
  {
    ""word"": ""rotate"",
    ""rhymes"": [
      ""state"",
      ""rate"",
      ""plate"",
      ""great"",
      ""mandate"",
      ""estate"",
      ""gate"",
      ""indicate"",
      ""update"",
      ""wait"",
      ""debate""
    ],
    ""soundsLike"": [
      ""rate""
    ]
  },
  {
    ""word"": ""rough"",
    ""rhymes"": [
      ""stuff"",
      ""enough""
    ],
    ""soundsLike"": [
      ""roof""
    ]
  },
  {
    ""word"": ""round"",
    ""rhymes"": [
      ""around"",
      ""sound"",
      ""found"",
      ""surround""
    ],
    ""soundsLike"": [
      ""around"",
      ""rent"",
      ""surround""
    ]
  },
  {
    ""word"": ""route"",
    ""rhymes"": [
      ""suit"",
      ""about"",
      ""shoot"",
      ""fruit"",
      ""minute"",
      ""execute"",
      ""cute"",
      ""scout"",
      ""salute"",
      ""lawsuit""
    ],
    ""soundsLike"": [
      ""right"",
      ""write"",
      ""rude"",
      ""rate"",
      ""fruit""
    ]
  },
  {
    ""word"": ""royal"",
    ""rhymes"": [
      ""loyal""
    ],
    ""soundsLike"": [
      ""cruel"",
      ""rail""
    ]
  },
  {
    ""word"": ""rubber"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""rare"",
      ""rib""
    ]
  },
  {
    ""word"": ""rude"",
    ""rhymes"": [
      ""food"",
      ""attitude"",
      ""include"",
      ""exclude""
    ],
    ""soundsLike"": [
      ""road"",
      ""ride"",
      ""route"",
      ""rate""
    ]
  },
  {
    ""word"": ""rug"",
    ""rhymes"": [
      ""plug"",
      ""shrug""
    ],
    ""soundsLike"": [
      ""wreck"",
      ""rack""
    ]
  },
  {
    ""word"": ""rule"",
    ""rhymes"": [
      ""school"",
      ""pool"",
      ""cool"",
      ""tool"",
      ""fuel"",
      ""stool"",
      ""cruel"",
      ""mule"",
      ""jewel""
    ],
    ""soundsLike"": [
      ""real"",
      ""rail"",
      ""cruel""
    ]
  },
  {
    ""word"": ""run"",
    ""rhymes"": [
      ""one"",
      ""gun"",
      ""fun"",
      ""sun"",
      ""someone""
    ],
    ""soundsLike"": [
      ""around"",
      ""rain"",
      ""ring""
    ]
  },
  {
    ""word"": ""runway"",
    ""rhymes"": [
      ""day"",
      ""away"",
      ""way"",
      ""play"",
      ""say"",
      ""survey"",
      ""display"",
      ""stay"",
      ""delay"",
      ""holiday"",
      ""essay"",
      ""clay"",
      ""betray"",
      ""spray"",
      ""they"",
      ""okay"",
      ""tray"",
      ""obey"",
      ""today""
    ],
    ""soundsLike"": [
      ""renew"",
      ""arena""
    ]
  },
  {
    ""word"": ""rural"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""royal""
    ]
  },
  {
    ""word"": ""sad"",
    ""rhymes"": [
      ""add"",
      ""mad"",
      ""glad"",
      ""dad""
    ],
    ""soundsLike"": [
      ""side"",
      ""seed""
    ]
  },
  {
    ""word"": ""saddle"",
    ""rhymes"": [
      ""paddle""
    ],
    ""soundsLike"": [
      ""settle"",
      ""soda""
    ]
  },
  {
    ""word"": ""sadness"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""witness""
    ]
  },
  {
    ""word"": ""safe"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""say"",
      ""save""
    ]
  },
  {
    ""word"": ""sail"",
    ""rhymes"": [
      ""scale"",
      ""rail"",
      ""mail"",
      ""whale"",
      ""detail"",
      ""tail"",
      ""female"",
      ""unveil"",
      ""inhale""
    ],
    ""soundsLike"": [
      ""soul"",
      ""sell"",
      ""say"",
      ""scale""
    ]
  },
  {
    ""word"": ""salad"",
    ""rhymes"": [
      ""valid""
    ],
    ""soundsLike"": [
      ""solid"",
      ""slide"",
      ""salute""
    ]
  },
  {
    ""word"": ""salmon"",
    ""rhymes"": [],
    ""soundsLike"": []
  },
  {
    ""word"": ""salon"",
    ""rhymes"": [
      ""upon"",
      ""dawn"",
      ""spawn"",
      ""lawn""
    ],
    ""soundsLike"": [
      ""ceiling""
    ]
  },
  {
    ""word"": ""salt"",
    ""rhymes"": [
      ""vault"",
      ""fault"",
      ""assault""
    ],
    ""soundsLike"": [
      ""assault""
    ]
  },
  {
    ""word"": ""salute"",
    ""rhymes"": [
      ""suit"",
      ""shoot"",
      ""route"",
      ""fruit"",
      ""minute"",
      ""execute"",
      ""cute"",
      ""lawsuit""
    ],
    ""soundsLike"": [
      ""salt"",
      ""solid"",
      ""salad"",
      ""slight"",
      ""select"",
      ""slot"",
      ""isolate""
    ]
  },
  {
    ""word"": ""same"",
    ""rhymes"": [
      ""game"",
      ""name"",
      ""frame"",
      ""claim"",
      ""aim"",
      ""blame"",
      ""flame"",
      ""fame""
    ],
    ""soundsLike"": []
  },
  {
    ""word"": ""sample"",
    ""rhymes"": [
      ""example""
    ],
    ""soundsLike"": [
      ""simple"",
      ""symbol""
    ]
  },
  {
    ""word"": ""sand"",
    ""rhymes"": [
      ""hand"",
      ""stand"",
      ""demand"",
      ""brand"",
      ""expand""
    ],
    ""soundsLike"": [
      ""sound"",
      ""sad"",
      ""stand"",
      ""hand""
    ]
  },
  {
    ""word"": ""satisfy"",
    ""rhymes"": [
      ""fly"",
      ""eye"",
      ""high"",
      ""supply"",
      ""identify"",
      ""dry"",
      ""shy"",
      ""cry"",
      ""clarify"",
      ""try"",
      ""verify"",
      ""deny"",
      ""defy"",
      ""rely"",
      ""modify"",
      ""spy""
    ],
    ""soundsLike"": []
  },
  {
    ""word"": ""satoshi"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""city""
    ]
  },
  {
    ""word"": ""sauce"",
    ""rhymes"": [
      ""cross"",
      ""across"",
      ""boss"",
      ""toss""
    ],
    ""soundsLike"": []
  },
  {
    ""word"": ""sausage"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""usage"",
      ""sauce""
    ]
  },
  {
    ""word"": ""save"",
    ""rhymes"": [
      ""wave"",
      ""brave"",
      ""cave"",
      ""behave"",
      ""pave""
    ],
    ""soundsLike"": [
      ""say"",
      ""safe"",
      ""wave""
    ]
  },
  {
    ""word"": ""say"",
    ""rhymes"": [
      ""day"",
      ""away"",
      ""way"",
      ""play"",
      ""survey"",
      ""display"",
      ""stay"",
      ""delay"",
      ""holiday"",
      ""essay"",
      ""clay"",
      ""betray"",
      ""spray"",
      ""they"",
      ""okay"",
      ""tray"",
      ""obey"",
      ""today""
    ],
    ""soundsLike"": [
      ""sea""
    ]
  },
  {
    ""word"": ""scale"",
    ""rhymes"": [
      ""rail"",
      ""mail"",
      ""whale"",
      ""detail"",
      ""tail"",
      ""sail"",
      ""female"",
      ""unveil"",
      ""inhale""
    ],
    ""soundsLike"": [
      ""school"",
      ""skill"",
      ""skull"",
      ""sail"",
      ""circle"",
      ""cycle""
    ]
  },
  {
    ""word"": ""scan"",
    ""rhymes"": [
      ""man"",
      ""can"",
      ""fan"",
      ""van""
    ],
    ""soundsLike"": [
      ""skin"",
      ""scheme""
    ]
  },
  {
    ""word"": ""scare"",
    ""rhymes"": [
      ""air"",
      ""chair"",
      ""wear"",
      ""square"",
      ""share"",
      ""despair"",
      ""there"",
      ""hair"",
      ""spare"",
      ""aware"",
      ""pair"",
      ""where"",
      ""rare"",
      ""swear"",
      ""pear"",
      ""affair"",
      ""repair"",
      ""prepare"",
      ""glare"",
      ""warfare"",
      ""unfair"",
      ""unaware""
    ],
    ""soundsLike"": [
      ""square""
    ]
  },
  {
    ""word"": ""scatter"",
    ""rhymes"": [
      ""matter""
    ],
    ""soundsLike"": [
      ""scare"",
      ""skate"",
      ""scout""
    ]
  },
  {
    ""word"": ""scene"",
    ""rhymes"": [
      ""mean"",
      ""green"",
      ""bean"",
      ""machine"",
      ""keen"",
      ""screen"",
      ""clean"",
      ""between"",
      ""marine""
    ],
    ""soundsLike"": [
      ""sun"",
      ""soon"",
      ""sign""
    ]
  },
  {
    ""word"": ""scheme"",
    ""rhymes"": [
      ""dream"",
      ""cream"",
      ""team"",
      ""theme"",
      ""supreme""
    ],
    ""soundsLike"": [
      ""skin"",
      ""ski"",
      ""scan""
    ]
  },
  {
    ""word"": ""school"",
    ""rhymes"": [
      ""rule"",
      ""pool"",
      ""cool"",
      ""tool"",
      ""fuel"",
      ""stool"",
      ""cruel"",
      ""mule"",
      ""jewel""
    ],
    ""soundsLike"": [
      ""skill"",
      ""skull"",
      ""scale"",
      ""circle"",
      ""cycle""
    ]
  },
  {
    ""word"": ""science"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""sense"",
      ""since"",
      ""essence""
    ]
  },
  {
    ""word"": ""scissors"",
    ""rhymes"": [],
    ""soundsLike"": []
  },
  {
    ""word"": ""scorpion"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""screen""
    ]
  },
  {
    ""word"": ""scout"",
    ""rhymes"": [
      ""about"",
      ""route""
    ],
    ""soundsLike"": [
      ""skate""
    ]
  },
  {
    ""word"": ""scrap"",
    ""rhymes"": [
      ""snap"",
      ""trap"",
      ""gap"",
      ""wrap"",
      ""clap""
    ],
    ""soundsLike"": [
      ""scrub"",
      ""script"",
      ""syrup""
    ]
  },
  {
    ""word"": ""screen"",
    ""rhymes"": [
      ""mean"",
      ""green"",
      ""bean"",
      ""machine"",
      ""keen"",
      ""clean"",
      ""between"",
      ""scene"",
      ""marine""
    ],
    ""soundsLike"": [
      ""scheme"",
      ""siren"",
      ""scan""
    ]
  },
  {
    ""word"": ""script"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""scrap"",
      ""scrub"",
      ""secret"",
      ""skirt""
    ]
  },
  {
    ""word"": ""scrub"",
    ""rhymes"": [
      ""club"",
      ""hub""
    ],
    ""soundsLike"": [
      ""scrap"",
      ""syrup"",
      ""script""
    ]
  },
  {
    ""word"": ""sea"",
    ""rhymes"": [
      ""tree"",
      ""key"",
      ""flee"",
      ""degree"",
      ""debris"",
      ""agree"",
      ""fee"",
      ""ski"",
      ""three"",
      ""pony"",
      ""knee""
    ],
    ""soundsLike"": [
      ""say""
    ]
  },
  {
    ""word"": ""search"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""surge"",
      ""such"",
      ""arch""
    ]
  },
  {
    ""word"": ""season"",
    ""rhymes"": [
      ""reason""
    ],
    ""soundsLike"": [
      ""session"",
      ""siren""
    ]
  },
  {
    ""word"": ""seat"",
    ""rhymes"": [
      ""treat"",
      ""street"",
      ""elite"",
      ""sweet"",
      ""meat"",
      ""retreat"",
      ""athlete"",
      ""wheat"",
      ""repeat""
    ],
    ""soundsLike"": [
      ""sight"",
      ""suit"",
      ""seed""
    ]
  },
  {
    ""word"": ""second"",
    ""rhymes"": [],
    ""soundsLike"": []
  },
  {
    ""word"": ""secret"",
    ""rhymes"": [],
    ""soundsLike"": []
  },
  {
    ""word"": ""section"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""second"",
      ""session"",
      ""fiction""
    ]
  },
  {
    ""word"": ""security"",
    ""rhymes"": [
      ""purity""
    ],
    ""soundsLike"": []
  },
  {
    ""word"": ""seed"",
    ""rhymes"": [
      ""feed"",
      ""speed"",
      ""need""
    ],
    ""soundsLike"": [
      ""side"",
      ""sad"",
      ""seat""
    ]
  },
  {
    ""word"": ""seek"",
    ""rhymes"": [
      ""unique"",
      ""bleak"",
      ""speak"",
      ""creek"",
      ""antique""
    ],
    ""soundsLike"": [
      ""sick"",
      ""sock""
    ]
  },
  {
    ""word"": ""segment"",
    ""rhymes"": [
      ""present"",
      ""rent"",
      ""prevent"",
      ""tent"",
      ""orient"",
      ""cement"",
      ""frequent""
    ],
    ""soundsLike"": [
      ""cement"",
      ""second"",
      ""salmon""
    ]
  },
  {
    ""word"": ""select"",
    ""rhymes"": [
      ""project"",
      ""subject"",
      ""aspect"",
      ""object"",
      ""perfect"",
      ""reflect"",
      ""direct"",
      ""connect"",
      ""expect"",
      ""correct"",
      ""neglect"",
      ""suspect"",
      ""collect"",
      ""protect"",
      ""reject"",
      ""detect"",
      ""insect"",
      ""inject""
    ],
    ""soundsLike"": [
      ""salute"",
      ""collect"",
      ""salad""
    ]
  },
  {
    ""word"": ""sell"",
    ""rhymes"": [
      ""spell"",
      ""shell"",
      ""tell"",
      ""rebel"",
      ""hotel""
    ],
    ""soundsLike"": [
      ""soul"",
      ""sail""
    ]
  },
  {
    ""word"": ""seminar"",
    ""rhymes"": [
      ""bar"",
      ""car"",
      ""jar"",
      ""radar"",
      ""guitar"",
      ""jaguar"",
      ""cigar""
    ],
    ""soundsLike"": [
      ""similar"",
      ""salmon""
    ]
  },
  {
    ""word"": ""senior"",
    ""rhymes"": [],
    ""soundsLike"": []
  },
  {
    ""word"": ""sense"",
    ""rhymes"": [
      ""fence"",
      ""defense"",
      ""immense""
    ],
    ""soundsLike"": [
      ""since"",
      ""science"",
      ""essence""
    ]
  },
  {
    ""word"": ""sentence"",
    ""rhymes"": [],
    ""soundsLike"": []
  },
  {
    ""word"": ""series"",
    ""rhymes"": [],
    ""soundsLike"": []
  },
  {
    ""word"": ""service"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""surface"",
      ""survey""
    ]
  },
  {
    ""word"": ""session"",
    ""rhymes"": [
      ""question""
    ],
    ""soundsLike"": [
      ""section"",
      ""seven"",
      ""second"",
      ""season""
    ]
  },
  {
    ""word"": ""settle"",
    ""rhymes"": [
      ""metal""
    ],
    ""soundsLike"": [
      ""saddle"",
      ""still""
    ]
  },
  {
    ""word"": ""setup"",
    ""rhymes"": [
      ""cup""
    ],
    ""soundsLike"": [
      ""step""
    ]
  },
  {
    ""word"": ""seven"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""second"",
      ""session""
    ]
  },
  {
    ""word"": ""shadow"",
    ""rhymes"": [
      ""know"",
      ""snow"",
      ""throw"",
      ""hello"",
      ""grow"",
      ""potato"",
      ""toe"",
      ""glow"",
      ""slow"",
      ""tomato"",
      ""below"",
      ""radio"",
      ""meadow"",
      ""tomorrow"",
      ""borrow"",
      ""photo"",
      ""tornado"",
      ""buffalo""
    ],
    ""soundsLike"": [
      ""shed""
    ]
  },
  {
    ""word"": ""shaft"",
    ""rhymes"": [
      ""draft"",
      ""craft""
    ],
    ""soundsLike"": [
      ""shift"",
      ""shoot"",
      ""chef"",
      ""chat""
    ]
  },
  {
    ""word"": ""shallow"",
    ""rhymes"": [],
    ""soundsLike"": []
  },
  {
    ""word"": ""share"",
    ""rhymes"": [
      ""air"",
      ""chair"",
      ""wear"",
      ""square"",
      ""despair"",
      ""there"",
      ""hair"",
      ""spare"",
      ""aware"",
      ""pair"",
      ""where"",
      ""rare"",
      ""swear"",
      ""pear"",
      ""affair"",
      ""repair"",
      ""prepare"",
      ""glare"",
      ""scare"",
      ""warfare"",
      ""unfair"",
      ""unaware""
    ],
    ""soundsLike"": [
      ""sure"",
      ""chair""
    ]
  },
  {
    ""word"": ""shed"",
    ""rhymes"": [
      ""head"",
      ""spread"",
      ""bread"",
      ""ahead""
    ],
    ""soundsLike"": [
      ""shoot""
    ]
  },
  {
    ""word"": ""shell"",
    ""rhymes"": [
      ""spell"",
      ""tell"",
      ""sell"",
      ""rebel"",
      ""hotel""
    ],
    ""soundsLike"": []
  },
  {
    ""word"": ""sheriff"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""share"",
      ""chef""
    ]
  },
  {
    ""word"": ""shield"",
    ""rhymes"": [
      ""field""
    ],
    ""soundsLike"": [
      ""child""
    ]
  },
  {
    ""word"": ""shift"",
    ""rhymes"": [
      ""lift"",
      ""drift"",
      ""gift"",
      ""swift""
    ],
    ""soundsLike"": [
      ""shaft"",
      ""lift"",
      ""chef"",
      ""left"",
      ""shoot""
    ]
  },
  {
    ""word"": ""shine"",
    ""rhymes"": [
      ""design"",
      ""sign"",
      ""fine"",
      ""wine"",
      ""genuine"",
      ""decline"",
      ""define"",
      ""online"",
      ""combine""
    ],
    ""soundsLike"": [
      ""shy""
    ]
  },
  {
    ""word"": ""ship"",
    ""rhymes"": [
      ""trip"",
      ""tip"",
      ""clip"",
      ""flip"",
      ""whip"",
      ""drip"",
      ""hip"",
      ""equip""
    ],
    ""soundsLike"": [
      ""shop"",
      ""cheap"",
      ""hip""
    ]
  },
  {
    ""word"": ""shiver"",
    ""rhymes"": [
      ""river"",
      ""deliver""
    ],
    ""soundsLike"": [
      ""river"",
      ""share"",
      ""shove"",
      ""hover""
    ]
  },
  {
    ""word"": ""shock"",
    ""rhymes"": [
      ""stock"",
      ""talk"",
      ""lock"",
      ""clock"",
      ""walk"",
      ""hawk"",
      ""flock"",
      ""knock"",
      ""chalk"",
      ""sock"",
      ""unlock""
    ],
    ""soundsLike"": [
      ""chalk"",
      ""check""
    ]
  },
  {
    ""word"": ""shoe"",
    ""rhymes"": [
      ""blue"",
      ""into"",
      ""you"",
      ""view"",
      ""review"",
      ""true"",
      ""two"",
      ""crew"",
      ""tissue"",
      ""few"",
      ""glue"",
      ""bamboo"",
      ""undo"",
      ""tattoo"",
      ""renew"",
      ""canoe"",
      ""zoo""
    ],
    ""soundsLike"": [
      ""shy""
    ]
  },
  {
    ""word"": ""shoot"",
    ""rhymes"": [
      ""suit"",
      ""route"",
      ""fruit"",
      ""minute"",
      ""execute"",
      ""cute"",
      ""salute"",
      ""lawsuit""
    ],
    ""soundsLike"": [
      ""shoe"",
      ""shed""
    ]
  },
  {
    ""word"": ""shop"",
    ""rhymes"": [
      ""top"",
      ""drop"",
      ""crop"",
      ""swap"",
      ""laptop""
    ],
    ""soundsLike"": [
      ""ship"",
      ""cheap""
    ]
  },
  {
    ""word"": ""short"",
    ""rhymes"": [
      ""report"",
      ""sport"",
      ""sort"",
      ""airport""
    ],
    ""soundsLike"": [
      ""sort"",
      ""heart""
    ]
  },
  {
    ""word"": ""shoulder"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""shield""
    ]
  },
  {
    ""word"": ""shove"",
    ""rhymes"": [
      ""love"",
      ""above"",
      ""dove"",
      ""glove""
    ],
    ""soundsLike"": [
      ""love"",
      ""chef"",
      ""shiver"",
      ""live""
    ]
  },
  {
    ""word"": ""shrimp"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""ship"",
      ""jump"",
      ""trip"",
      ""trim""
    ]
  },
  {
    ""word"": ""shrug"",
    ""rhymes"": [
      ""plug"",
      ""rug""
    ],
    ""soundsLike"": [
      ""truck""
    ]
  },
  {
    ""word"": ""shuffle"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""rifle""
    ]
  },
  {
    ""word"": ""shy"",
    ""rhymes"": [
      ""fly"",
      ""eye"",
      ""high"",
      ""supply"",
      ""identify"",
      ""dry"",
      ""cry"",
      ""clarify"",
      ""try"",
      ""verify"",
      ""deny"",
      ""defy"",
      ""rely"",
      ""modify"",
      ""satisfy"",
      ""spy""
    ],
    ""soundsLike"": [
      ""shoe""
    ]
  },
  {
    ""word"": ""sibling"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""ceiling""
    ]
  },
  {
    ""word"": ""sick"",
    ""rhymes"": [
      ""stick"",
      ""trick"",
      ""kick"",
      ""quick"",
      ""click"",
      ""brick"",
      ""picnic""
    ],
    ""soundsLike"": [
      ""seek"",
      ""sock""
    ]
  },
  {
    ""word"": ""side"",
    ""rhymes"": [
      ""provide"",
      ""ride"",
      ""slide"",
      ""pride"",
      ""tide"",
      ""guide"",
      ""decide"",
      ""divide"",
      ""wide"",
      ""glide"",
      ""inside"",
      ""outside""
    ],
    ""soundsLike"": [
      ""sad"",
      ""sight"",
      ""seed""
    ]
  },
  {
    ""word"": ""siege"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""sea"",
      ""such"",
      ""surge""
    ]
  },
  {
    ""word"": ""sight"",
    ""rhymes"": [
      ""light"",
      ""right"",
      ""night"",
      ""write"",
      ""flight"",
      ""bright"",
      ""slight"",
      ""height"",
      ""kite"",
      ""invite"",
      ""excite"",
      ""midnight""
    ],
    ""soundsLike"": [
      ""side"",
      ""seat"",
      ""suit""
    ]
  },
  {
    ""word"": ""sign"",
    ""rhymes"": [
      ""design"",
      ""fine"",
      ""wine"",
      ""genuine"",
      ""shine"",
      ""decline"",
      ""define"",
      ""online"",
      ""combine""
    ],
    ""soundsLike"": [
      ""sun"",
      ""soon"",
      ""scene""
    ]
  },
  {
    ""word"": ""silent"",
    ""rhymes"": [],
    ""soundsLike"": []
  },
  {
    ""word"": ""silk"",
    ""rhymes"": [
      ""milk""
    ],
    ""soundsLike"": [
      ""sick"",
      ""silly""
    ]
  },
  {
    ""word"": ""silly"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""silk""
    ]
  },
  {
    ""word"": ""silver"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""solve"",
      ""solar""
    ]
  },
  {
    ""word"": ""similar"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""simple""
    ]
  },
  {
    ""word"": ""simple"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""symbol"",
      ""sample"",
      ""stumble"",
      ""humble""
    ]
  },
  {
    ""word"": ""since"",
    ""rhymes"": [
      ""convince""
    ],
    ""soundsLike"": [
      ""sense"",
      ""science"",
      ""essence""
    ]
  },
  {
    ""word"": ""sing"",
    ""rhymes"": [
      ""ring"",
      ""bring"",
      ""spring"",
      ""swing"",
      ""sting"",
      ""thing"",
      ""wing""
    ],
    ""soundsLike"": [
      ""song"",
      ""sun""
    ]
  },
  {
    ""word"": ""siren"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""screen""
    ]
  },
  {
    ""word"": ""sister"",
    ""rhymes"": [],
    ""soundsLike"": []
  },
  {
    ""word"": ""situate"",
    ""rhymes"": [
      ""state"",
      ""rate"",
      ""plate"",
      ""great"",
      ""mandate"",
      ""estate"",
      ""gate"",
      ""indicate"",
      ""update"",
      ""wait"",
      ""debate""
    ],
    ""soundsLike"": [
      ""salute"",
      ""select""
    ]
  },
  {
    ""word"": ""six"",
    ""rhymes"": [
      ""fix"",
      ""mix""
    ],
    ""soundsLike"": [
      ""sick""
    ]
  },
  {
    ""word"": ""size"",
    ""rhymes"": [
      ""exercise"",
      ""demise"",
      ""wise"",
      ""surprise"",
      ""prize""
    ],
    ""soundsLike"": []
  },
  {
    ""word"": ""skate"",
    ""rhymes"": [
      ""state"",
      ""rate"",
      ""plate"",
      ""great"",
      ""mandate"",
      ""estate"",
      ""gate"",
      ""indicate"",
      ""update"",
      ""wait"",
      ""debate""
    ],
    ""soundsLike"": [
      ""scout""
    ]
  },
  {
    ""word"": ""sketch"",
    ""rhymes"": [
      ""fetch""
    ],
    ""soundsLike"": [
      ""skin"",
      ""skill"",
      ""such"",
      ""skull"",
      ""school"",
      ""scare"",
      ""scale"",
      ""scan""
    ]
  },
  {
    ""word"": ""ski"",
    ""rhymes"": [
      ""tree"",
      ""sea"",
      ""key"",
      ""flee"",
      ""degree"",
      ""debris"",
      ""agree"",
      ""fee"",
      ""three"",
      ""pony"",
      ""knee""
    ],
    ""soundsLike"": []
  },
  {
    ""word"": ""skill"",
    ""rhymes"": [
      ""will"",
      ""still"",
      ""drill"",
      ""hill"",
      ""pill"",
      ""ill"",
      ""until""
    ],
    ""soundsLike"": [
      ""skull"",
      ""school"",
      ""scale"",
      ""circle"",
      ""cycle"",
      ""sell""
    ]
  },
  {
    ""word"": ""skin"",
    ""rhymes"": [
      ""begin"",
      ""spin"",
      ""win"",
      ""when"",
      ""twin"",
      ""violin""
    ],
    ""soundsLike"": [
      ""scan"",
      ""scheme"",
      ""second""
    ]
  },
  {
    ""word"": ""skirt"",
    ""rhymes"": [
      ""desert"",
      ""hurt"",
      ""alert"",
      ""concert"",
      ""divert"",
      ""dirt""
    ],
    ""soundsLike"": [
      ""sort"",
      ""scout""
    ]
  },
  {
    ""word"": ""skull"",
    ""rhymes"": [
      ""will""
    ],
    ""soundsLike"": [
      ""skill"",
      ""school"",
      ""scale"",
      ""circle"",
      ""cycle""
    ]
  },
  {
    ""word"": ""slab"",
    ""rhymes"": [
      ""lab"",
      ""grab""
    ],
    ""soundsLike"": [
      ""sleep"",
      ""slow""
    ]
  },
  {
    ""word"": ""slam"",
    ""rhymes"": [
      ""program"",
      ""diagram"",
      ""cram""
    ],
    ""soundsLike"": [
      ""slim"",
      ""same""
    ]
  },
  {
    ""word"": ""sleep"",
    ""rhymes"": [
      ""keep"",
      ""cheap""
    ],
    ""soundsLike"": [
      ""slab""
    ]
  },
  {
    ""word"": ""slender"",
    ""rhymes"": [
      ""render"",
      ""vendor""
    ],
    ""soundsLike"": []
  },
  {
    ""word"": ""slice"",
    ""rhymes"": [
      ""ice"",
      ""advice"",
      ""device"",
      ""nice"",
      ""price"",
      ""rice"",
      ""dice"",
      ""spice"",
      ""twice""
    ],
    ""soundsLike"": [
      ""size"",
      ""sauce""
    ]
  },
  {
    ""word"": ""slide"",
    ""rhymes"": [
      ""side"",
      ""provide"",
      ""ride"",
      ""pride"",
      ""tide"",
      ""guide"",
      ""decide"",
      ""divide"",
      ""wide"",
      ""glide"",
      ""inside"",
      ""outside""
    ],
    ""soundsLike"": [
      ""slight"",
      ""side"",
      ""slot"",
      ""sad"",
      ""seed"",
      ""salad""
    ]
  },
  {
    ""word"": ""slight"",
    ""rhymes"": [
      ""light"",
      ""right"",
      ""night"",
      ""write"",
      ""sight"",
      ""flight"",
      ""bright"",
      ""height"",
      ""kite"",
      ""invite"",
      ""excite"",
      ""midnight""
    ],
    ""soundsLike"": [
      ""slide"",
      ""slot"",
      ""sight"",
      ""seat"",
      ""salute""
    ]
  },
  {
    ""word"": ""slim"",
    ""rhymes"": [
      ""swim"",
      ""trim"",
      ""limb"",
      ""gym""
    ],
    ""soundsLike"": [
      ""slam"",
      ""swim"",
      ""ceiling""
    ]
  },
  {
    ""word"": ""slogan"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""second""
    ]
  },
  {
    ""word"": ""slot"",
    ""rhymes"": [
      ""spot"",
      ""thought"",
      ""robot"",
      ""caught""
    ],
    ""soundsLike"": [
      ""slight"",
      ""slide""
    ]
  },
  {
    ""word"": ""slow"",
    ""rhymes"": [
      ""know"",
      ""snow"",
      ""throw"",
      ""hello"",
      ""grow"",
      ""shadow"",
      ""potato"",
      ""toe"",
      ""glow"",
      ""tomato"",
      ""below"",
      ""radio"",
      ""meadow"",
      ""tomorrow"",
      ""borrow"",
      ""photo"",
      ""tornado"",
      ""buffalo""
    ],
    ""soundsLike"": []
  },
  {
    ""word"": ""slush"",
    ""rhymes"": [
      ""flush"",
      ""crush"",
      ""brush"",
      ""blush""
    ],
    ""soundsLike"": [
      ""slow"",
      ""slice"",
      ""slight"",
      ""slot""
    ]
  },
  {
    ""word"": ""small"",
    ""rhymes"": [
      ""all"",
      ""ball"",
      ""call"",
      ""fall"",
      ""wall"",
      ""recall"",
      ""alcohol"",
      ""doll"",
      ""install"",
      ""crawl""
    ],
    ""soundsLike"": [
      ""smile"",
      ""sell"",
      ""sail""
    ]
  },
  {
    ""word"": ""smart"",
    ""rhymes"": [
      ""art"",
      ""heart"",
      ""start"",
      ""apart"",
      ""cart"",
      ""depart""
    ],
    ""soundsLike"": [
      ""sort"",
      ""start"",
      ""sword""
    ]
  },
  {
    ""word"": ""smile"",
    ""rhymes"": [
      ""style"",
      ""file"",
      ""trial"",
      ""aisle"",
      ""exile"",
      ""dial""
    ],
    ""soundsLike"": [
      ""small"",
      ""sell"",
      ""sail""
    ]
  },
  {
    ""word"": ""smoke"",
    ""rhymes"": [
      ""oak"",
      ""evoke"",
      ""joke""
    ],
    ""soundsLike"": [
      ""snake"",
      ""snack"",
      ""seek""
    ]
  },
  {
    ""word"": ""smooth"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""smile"",
      ""small""
    ]
  },
  {
    ""word"": ""snack"",
    ""rhymes"": [
      ""black"",
      ""track"",
      ""attack"",
      ""crack"",
      ""rack""
    ],
    ""soundsLike"": [
      ""snake"",
      ""smoke"",
      ""seek""
    ]
  },
  {
    ""word"": ""snake"",
    ""rhymes"": [
      ""awake"",
      ""cake"",
      ""make"",
      ""mistake"",
      ""lake"",
      ""steak""
    ],
    ""soundsLike"": [
      ""snack"",
      ""smoke"",
      ""seek""
    ]
  },
  {
    ""word"": ""snap"",
    ""rhymes"": [
      ""trap"",
      ""gap"",
      ""wrap"",
      ""scrap"",
      ""clap""
    ],
    ""soundsLike"": [
      ""snow""
    ]
  },
  {
    ""word"": ""sniff"",
    ""rhymes"": [
      ""cliff""
    ],
    ""soundsLike"": [
      ""enough"",
      ""snow"",
      ""safe""
    ]
  },
  {
    ""word"": ""snow"",
    ""rhymes"": [
      ""know"",
      ""throw"",
      ""hello"",
      ""grow"",
      ""shadow"",
      ""potato"",
      ""toe"",
      ""glow"",
      ""slow"",
      ""tomato"",
      ""below"",
      ""radio"",
      ""meadow"",
      ""tomorrow"",
      ""borrow"",
      ""photo"",
      ""tornado"",
      ""buffalo""
    ],
    ""soundsLike"": []
  },
  {
    ""word"": ""soap"",
    ""rhymes"": [
      ""hope"",
      ""envelope""
    ],
    ""soundsLike"": [
      ""soup""
    ]
  },
  {
    ""word"": ""soccer"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""sock""
    ]
  },
  {
    ""word"": ""social"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""special"",
      ""settle"",
      ""spatial""
    ]
  },
  {
    ""word"": ""sock"",
    ""rhymes"": [
      ""stock"",
      ""talk"",
      ""lock"",
      ""clock"",
      ""walk"",
      ""shock"",
      ""hawk"",
      ""flock"",
      ""knock"",
      ""chalk"",
      ""unlock""
    ],
    ""soundsLike"": [
      ""sick"",
      ""seek""
    ]
  },
  {
    ""word"": ""soda"",
    ""rhymes"": [],
    ""soundsLike"": []
  },
  {
    ""word"": ""soft"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""safe""
    ]
  },
  {
    ""word"": ""solar"",
    ""rhymes"": [
      ""polar""
    ],
    ""soundsLike"": [
      ""soul"",
      ""soldier"",
      ""celery""
    ]
  },
  {
    ""word"": ""soldier"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""solar"",
      ""silver""
    ]
  },
  {
    ""word"": ""solid"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""salad"",
      ""slide"",
      ""salute""
    ]
  },
  {
    ""word"": ""solution"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""session"",
      ""section""
    ]
  },
  {
    ""word"": ""solve"",
    ""rhymes"": [
      ""evolve"",
      ""involve""
    ],
    ""soundsLike"": [
      ""silly""
    ]
  },
  {
    ""word"": ""someone"",
    ""rhymes"": [
      ""run"",
      ""one"",
      ""gun"",
      ""fun"",
      ""sun""
    ],
    ""soundsLike"": [
      ""salmon""
    ]
  },
  {
    ""word"": ""song"",
    ""rhymes"": [
      ""long"",
      ""strong"",
      ""wrong""
    ],
    ""soundsLike"": [
      ""sing"",
      ""same"",
      ""scene""
    ]
  },
  {
    ""word"": ""soon"",
    ""rhymes"": [
      ""moon"",
      ""spoon"",
      ""immune"",
      ""dune"",
      ""raccoon""
    ],
    ""soundsLike"": [
      ""sun"",
      ""scene"",
      ""sign""
    ]
  },
  {
    ""word"": ""sorry"",
    ""rhymes"": [],
    ""soundsLike"": []
  },
  {
    ""word"": ""sort"",
    ""rhymes"": [
      ""short"",
      ""report"",
      ""sport"",
      ""airport""
    ],
    ""soundsLike"": [
      ""sword"",
      ""sport"",
      ""skirt"",
      ""short""
    ]
  },
  {
    ""word"": ""soul"",
    ""rhymes"": [
      ""control"",
      ""hole"",
      ""pole"",
      ""enroll"",
      ""patrol""
    ],
    ""soundsLike"": [
      ""sell"",
      ""sail"",
      ""solar""
    ]
  },
  {
    ""word"": ""sound"",
    ""rhymes"": [
      ""around"",
      ""round"",
      ""found"",
      ""surround""
    ],
    ""soundsLike"": [
      ""sand"",
      ""surround""
    ]
  },
  {
    ""word"": ""soup"",
    ""rhymes"": [
      ""group"",
      ""loop""
    ],
    ""soundsLike"": [
      ""soap""
    ]
  },
  {
    ""word"": ""source"",
    ""rhymes"": [
      ""force"",
      ""course"",
      ""horse"",
      ""endorse"",
      ""enforce"",
      ""divorce""
    ],
    ""soundsLike"": [
      ""sauce"",
      ""horse"",
      ""sorry""
    ]
  },
  {
    ""word"": ""south"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""sauce"",
      ""sock""
    ]
  },
  {
    ""word"": ""space"",
    ""rhymes"": [
      ""case"",
      ""grace"",
      ""face"",
      ""base"",
      ""place"",
      ""race"",
      ""embrace"",
      ""chase"",
      ""replace"",
      ""erase""
    ],
    ""soundsLike"": [
      ""spice"",
      ""sauce""
    ]
  },
  {
    ""word"": ""spare"",
    ""rhymes"": [
      ""air"",
      ""chair"",
      ""wear"",
      ""square"",
      ""share"",
      ""despair"",
      ""there"",
      ""hair"",
      ""aware"",
      ""pair"",
      ""where"",
      ""rare"",
      ""swear"",
      ""pear"",
      ""affair"",
      ""repair"",
      ""prepare"",
      ""glare"",
      ""scare"",
      ""warfare"",
      ""unfair"",
      ""unaware""
    ],
    ""soundsLike"": [
      ""swear"",
      ""appear""
    ]
  },
  {
    ""word"": ""spatial"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""special"",
      ""social"",
      ""spell"",
      ""settle""
    ]
  },
  {
    ""word"": ""spawn"",
    ""rhymes"": [
      ""upon"",
      ""dawn"",
      ""salon"",
      ""lawn""
    ],
    ""soundsLike"": [
      ""spin"",
      ""spoon"",
      ""upon""
    ]
  },
  {
    ""word"": ""speak"",
    ""rhymes"": [
      ""seek"",
      ""unique"",
      ""bleak"",
      ""creek"",
      ""antique""
    ],
    ""soundsLike"": [
      ""spike"",
      ""seek""
    ]
  },
  {
    ""word"": ""special"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""spatial"",
      ""social"",
      ""spell"",
      ""settle""
    ]
  },
  {
    ""word"": ""speed"",
    ""rhymes"": [
      ""feed"",
      ""seed"",
      ""need""
    ],
    ""soundsLike"": [
      ""spot"",
      ""seed"",
      ""side"",
      ""sad"",
      ""seat""
    ]
  },
  {
    ""word"": ""spell"",
    ""rhymes"": [
      ""shell"",
      ""tell"",
      ""sell"",
      ""rebel"",
      ""hotel""
    ],
    ""soundsLike"": [
      ""spoil"",
      ""sell"",
      ""apple""
    ]
  },
  {
    ""word"": ""spend"",
    ""rhymes"": [
      ""end"",
      ""friend"",
      ""lend"",
      ""attend"",
      ""trend"",
      ""extend"",
      ""depend"",
      ""weekend""
    ],
    ""soundsLike"": [
      ""spin"",
      ""sound"",
      ""sand"",
      ""speed"",
      ""spoon"",
      ""spawn""
    ]
  },
  {
    ""word"": ""sphere"",
    ""rhymes"": [
      ""year"",
      ""deer"",
      ""pioneer"",
      ""appear"",
      ""near""
    ],
    ""soundsLike"": [
      ""affair""
    ]
  },
  {
    ""word"": ""spice"",
    ""rhymes"": [
      ""ice"",
      ""advice"",
      ""device"",
      ""nice"",
      ""price"",
      ""rice"",
      ""dice"",
      ""slice"",
      ""twice""
    ],
    ""soundsLike"": [
      ""space"",
      ""spy"",
      ""size"",
      ""sauce""
    ]
  },
  {
    ""word"": ""spider"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""speed"",
      ""spare""
    ]
  },
  {
    ""word"": ""spike"",
    ""rhymes"": [
      ""strike"",
      ""like"",
      ""bike""
    ],
    ""soundsLike"": [
      ""speak"",
      ""spy"",
      ""seek""
    ]
  },
  {
    ""word"": ""spin"",
    ""rhymes"": [
      ""begin"",
      ""skin"",
      ""win"",
      ""when"",
      ""twin"",
      ""violin""
    ],
    ""soundsLike"": [
      ""spoon"",
      ""spawn"",
      ""sun"",
      ""spend""
    ]
  },
  {
    ""word"": ""spirit"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""sport"",
      ""spread"",
      ""secret""
    ]
  },
  {
    ""word"": ""split"",
    ""rhymes"": [
      ""fit"",
      ""grit"",
      ""quit"",
      ""kit"",
      ""submit"",
      ""admit"",
      ""permit"",
      ""omit""
    ],
    ""soundsLike"": [
      ""spot"",
      ""slight"",
      ""slot"",
      ""solid""
    ]
  },
  {
    ""word"": ""spoil"",
    ""rhymes"": [
      ""oil"",
      ""foil"",
      ""coil"",
      ""boil""
    ],
    ""soundsLike"": [
      ""spell"",
      ""soul"",
      ""sell"",
      ""sail""
    ]
  },
  {
    ""word"": ""sponsor"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""spawn""
    ]
  },
  {
    ""word"": ""spoon"",
    ""rhymes"": [
      ""moon"",
      ""soon"",
      ""immune"",
      ""dune"",
      ""raccoon""
    ],
    ""soundsLike"": [
      ""spin"",
      ""spawn"",
      ""soon"",
      ""scene"",
      ""sign""
    ]
  },
  {
    ""word"": ""sport"",
    ""rhymes"": [
      ""short"",
      ""report"",
      ""sort"",
      ""airport""
    ],
    ""soundsLike"": [
      ""sort"",
      ""spot"",
      ""apart"",
      ""spirit"",
      ""spare""
    ]
  },
  {
    ""word"": ""spot"",
    ""rhymes"": [
      ""thought"",
      ""slot"",
      ""robot"",
      ""caught""
    ],
    ""soundsLike"": [
      ""speed"",
      ""sport""
    ]
  },
  {
    ""word"": ""spray"",
    ""rhymes"": [
      ""day"",
      ""away"",
      ""way"",
      ""play"",
      ""say"",
      ""survey"",
      ""display"",
      ""stay"",
      ""delay"",
      ""holiday"",
      ""essay"",
      ""clay"",
      ""betray"",
      ""they"",
      ""okay"",
      ""tray"",
      ""obey"",
      ""today""
    ],
    ""soundsLike"": [
      ""sorry""
    ]
  },
  {
    ""word"": ""spread"",
    ""rhymes"": [
      ""head"",
      ""shed"",
      ""bread"",
      ""ahead""
    ],
    ""soundsLike"": [
      ""speed"",
      ""spray"",
      ""spirit""
    ]
  },
  {
    ""word"": ""spring"",
    ""rhymes"": [
      ""ring"",
      ""bring"",
      ""swing"",
      ""sting"",
      ""thing"",
      ""wing"",
      ""sing""
    ],
    ""soundsLike"": [
      ""spin"",
      ""spray""
    ]
  },
  {
    ""word"": ""spy"",
    ""rhymes"": [
      ""fly"",
      ""eye"",
      ""high"",
      ""supply"",
      ""identify"",
      ""dry"",
      ""shy"",
      ""cry"",
      ""clarify"",
      ""try"",
      ""verify"",
      ""deny"",
      ""defy"",
      ""rely"",
      ""modify"",
      ""satisfy""
    ],
    ""soundsLike"": [
      ""spice"",
      ""spike""
    ]
  },
  {
    ""word"": ""square"",
    ""rhymes"": [
      ""air"",
      ""chair"",
      ""wear"",
      ""share"",
      ""despair"",
      ""there"",
      ""hair"",
      ""spare"",
      ""aware"",
      ""pair"",
      ""where"",
      ""rare"",
      ""swear"",
      ""pear"",
      ""affair"",
      ""repair"",
      ""prepare"",
      ""glare"",
      ""scare"",
      ""warfare"",
      ""unfair"",
      ""unaware""
    ],
    ""soundsLike"": [
      ""scare"",
      ""swear"",
      ""squeeze"",
      ""acquire""
    ]
  },
  {
    ""word"": ""squeeze"",
    ""rhymes"": [
      ""disease"",
      ""cheese"",
      ""breeze"",
      ""please""
    ],
    ""soundsLike"": [
      ""square""
    ]
  },
  {
    ""word"": ""squirrel"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""skull"",
      ""equal"",
      ""skill""
    ]
  },
  {
    ""word"": ""stable"",
    ""rhymes"": [
      ""table"",
      ""label"",
      ""enable"",
      ""able"",
      ""cable"",
      ""unable""
    ],
    ""soundsLike"": [
      ""stumble""
    ]
  },
  {
    ""word"": ""stadium"",
    ""rhymes"": [],
    ""soundsLike"": []
  },
  {
    ""word"": ""staff"",
    ""rhymes"": [
      ""half"",
      ""laugh"",
      ""giraffe""
    ],
    ""soundsLike"": [
      ""stuff"",
      ""stay"",
      ""safe""
    ]
  },
  {
    ""word"": ""stage"",
    ""rhymes"": [
      ""gauge"",
      ""age"",
      ""engage"",
      ""page"",
      ""cage"",
      ""wage""
    ],
    ""soundsLike"": [
      ""stay"",
      ""state"",
      ""sting"",
      ""steak"",
      ""siege""
    ]
  },
  {
    ""word"": ""stairs"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""start""
    ]
  },
  {
    ""word"": ""stamp"",
    ""rhymes"": [
      ""camp"",
      ""lamp"",
      ""ramp"",
      ""damp""
    ],
    ""soundsLike"": [
      ""step"",
      ""stem"",
      ""swamp""
    ]
  },
  {
    ""word"": ""stand"",
    ""rhymes"": [
      ""hand"",
      ""demand"",
      ""brand"",
      ""expand"",
      ""sand""
    ],
    ""soundsLike"": [
      ""sand"",
      ""sound""
    ]
  },
  {
    ""word"": ""start"",
    ""rhymes"": [
      ""art"",
      ""smart"",
      ""heart"",
      ""apart"",
      ""cart"",
      ""depart""
    ],
    ""soundsLike"": [
      ""story"",
      ""sort"",
      ""smart"",
      ""state"",
      ""stairs""
    ]
  },
  {
    ""word"": ""state"",
    ""rhymes"": [
      ""rate"",
      ""plate"",
      ""great"",
      ""mandate"",
      ""estate"",
      ""gate"",
      ""indicate"",
      ""update"",
      ""wait"",
      ""debate""
    ],
    ""soundsLike"": [
      ""stay"",
      ""estate""
    ]
  },
  {
    ""word"": ""stay"",
    ""rhymes"": [
      ""day"",
      ""away"",
      ""way"",
      ""play"",
      ""say"",
      ""survey"",
      ""display"",
      ""delay"",
      ""holiday"",
      ""essay"",
      ""clay"",
      ""betray"",
      ""spray"",
      ""they"",
      ""okay"",
      ""tray"",
      ""obey"",
      ""today""
    ],
    ""soundsLike"": [
      ""say"",
      ""state"",
      ""stage"",
      ""steak""
    ]
  },
  {
    ""word"": ""steak"",
    ""rhymes"": [
      ""awake"",
      ""cake"",
      ""make"",
      ""snake"",
      ""mistake"",
      ""lake""
    ],
    ""soundsLike"": [
      ""stick"",
      ""stock"",
      ""stay""
    ]
  },
  {
    ""word"": ""steel"",
    ""rhymes"": [
      ""deal"",
      ""wheel"",
      ""feel"",
      ""real"",
      ""reveal""
    ],
    ""soundsLike"": [
      ""style"",
      ""still"",
      ""stool"",
      ""sting""
    ]
  },
  {
    ""word"": ""stem"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""sting"",
      ""stone""
    ]
  },
  {
    ""word"": ""step"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""setup""
    ]
  },
  {
    ""word"": ""stereo"",
    ""rhymes"": [
      ""know"",
      ""snow"",
      ""throw"",
      ""hello"",
      ""grow"",
      ""shadow"",
      ""potato"",
      ""toe"",
      ""glow"",
      ""slow"",
      ""tomato"",
      ""below"",
      ""radio"",
      ""meadow"",
      ""tomorrow"",
      ""borrow"",
      ""photo"",
      ""tornado"",
      ""buffalo""
    ],
    ""soundsLike"": [
      ""story"",
      ""stairs""
    ]
  },
  {
    ""word"": ""stick"",
    ""rhymes"": [
      ""trick"",
      ""kick"",
      ""quick"",
      ""sick"",
      ""click"",
      ""brick"",
      ""picnic""
    ],
    ""soundsLike"": [
      ""steak"",
      ""stock"",
      ""sick""
    ]
  },
  {
    ""word"": ""still"",
    ""rhymes"": [
      ""will"",
      ""drill"",
      ""hill"",
      ""skill"",
      ""pill"",
      ""ill"",
      ""until""
    ],
    ""soundsLike"": [
      ""style"",
      ""steel"",
      ""stool"",
      ""sting"",
      ""sell"",
      ""settle""
    ]
  },
  {
    ""word"": ""sting"",
    ""rhymes"": [
      ""ring"",
      ""bring"",
      ""spring"",
      ""swing"",
      ""thing"",
      ""wing"",
      ""sing""
    ],
    ""soundsLike"": [
      ""stem"",
      ""still"",
      ""sing"",
      ""stone"",
      ""swing""
    ]
  },
  {
    ""word"": ""stock"",
    ""rhymes"": [
      ""talk"",
      ""lock"",
      ""clock"",
      ""walk"",
      ""shock"",
      ""hawk"",
      ""flock"",
      ""knock"",
      ""chalk"",
      ""sock"",
      ""unlock""
    ],
    ""soundsLike"": [
      ""stick"",
      ""steak"",
      ""sock""
    ]
  },
  {
    ""word"": ""stomach"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""stem""
    ]
  },
  {
    ""word"": ""stone"",
    ""rhymes"": [
      ""bone"",
      ""zone"",
      ""tone"",
      ""loan"",
      ""phone"",
      ""own"",
      ""alone"",
      ""ozone"",
      ""unknown""
    ],
    ""soundsLike"": [
      ""sting"",
      ""stem"",
      ""certain""
    ]
  },
  {
    ""word"": ""stool"",
    ""rhymes"": [
      ""school"",
      ""rule"",
      ""pool"",
      ""cool"",
      ""tool"",
      ""fuel"",
      ""cruel"",
      ""mule"",
      ""jewel""
    ],
    ""soundsLike"": [
      ""style"",
      ""still"",
      ""steel"",
      ""sting"",
      ""school"",
      ""sail""
    ]
  },
  {
    ""word"": ""story"",
    ""rhymes"": [
      ""glory"",
      ""category""
    ],
    ""soundsLike"": [
      ""start"",
      ""sorry""
    ]
  },
  {
    ""word"": ""stove"",
    ""rhymes"": [
      ""dove""
    ],
    ""soundsLike"": [
      ""stay"",
      ""save"",
      ""sting"",
      ""staff""
    ]
  },
  {
    ""word"": ""strategy"",
    ""rhymes"": [],
    ""soundsLike"": []
  },
  {
    ""word"": ""street"",
    ""rhymes"": [
      ""seat"",
      ""treat"",
      ""elite"",
      ""sweet"",
      ""meat"",
      ""retreat"",
      ""athlete"",
      ""wheat"",
      ""repeat""
    ],
    ""soundsLike"": [
      ""state""
    ]
  },
  {
    ""word"": ""strike"",
    ""rhymes"": [
      ""like"",
      ""spike"",
      ""bike""
    ],
    ""soundsLike"": [
      ""stick"",
      ""steak"",
      ""stock""
    ]
  },
  {
    ""word"": ""strong"",
    ""rhymes"": [
      ""long"",
      ""wrong"",
      ""song""
    ],
    ""soundsLike"": [
      ""sting""
    ]
  },
  {
    ""word"": ""struggle"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""circle"",
      ""cereal""
    ]
  },
  {
    ""word"": ""student"",
    ""rhymes"": [],
    ""soundsLike"": []
  },
  {
    ""word"": ""stuff"",
    ""rhymes"": [
      ""rough"",
      ""enough""
    ],
    ""soundsLike"": [
      ""staff"",
      ""safe""
    ]
  },
  {
    ""word"": ""stumble"",
    ""rhymes"": [
      ""humble"",
      ""tumble"",
      ""crumble""
    ],
    ""soundsLike"": [
      ""symbol"",
      ""stable"",
      ""simple"",
      ""sample"",
      ""tumble"",
      ""humble""
    ]
  },
  {
    ""word"": ""style"",
    ""rhymes"": [
      ""file"",
      ""trial"",
      ""aisle"",
      ""smile"",
      ""exile"",
      ""dial""
    ],
    ""soundsLike"": [
      ""still"",
      ""steel"",
      ""stool"",
      ""sting"",
      ""sail""
    ]
  },
  {
    ""word"": ""subject"",
    ""rhymes"": [
      ""project"",
      ""aspect"",
      ""object"",
      ""perfect"",
      ""reflect"",
      ""direct"",
      ""connect"",
      ""expect"",
      ""correct"",
      ""neglect"",
      ""suspect"",
      ""collect"",
      ""select"",
      ""protect"",
      ""reject"",
      ""detect"",
      ""insect"",
      ""inject""
    ],
    ""soundsLike"": [
      ""object"",
      ""suspect"",
      ""select"",
      ""submit"",
      ""reject"",
      ""suggest""
    ]
  },
  {
    ""word"": ""submit"",
    ""rhymes"": [
      ""fit"",
      ""split"",
      ""grit"",
      ""quit"",
      ""kit"",
      ""admit"",
      ""permit"",
      ""omit""
    ],
    ""soundsLike"": [
      ""cement""
    ]
  },
  {
    ""word"": ""subway"",
    ""rhymes"": [
      ""day"",
      ""away"",
      ""way"",
      ""play"",
      ""say"",
      ""survey"",
      ""display"",
      ""stay"",
      ""delay"",
      ""holiday"",
      ""essay"",
      ""clay"",
      ""betray"",
      ""spray"",
      ""they"",
      ""okay"",
      ""tray"",
      ""obey"",
      ""today""
    ],
    ""soundsLike"": [
      ""supply""
    ]
  },
  {
    ""word"": ""success"",
    ""rhymes"": [
      ""process"",
      ""address"",
      ""access"",
      ""express"",
      ""dress"",
      ""guess"",
      ""excess"",
      ""bless""
    ],
    ""soundsLike"": [
      ""access"",
      ""excess""
    ]
  },
  {
    ""word"": ""such"",
    ""rhymes"": [
      ""clutch"",
      ""much"",
      ""dutch""
    ],
    ""soundsLike"": [
      ""sun"",
      ""search"",
      ""siege""
    ]
  },
  {
    ""word"": ""sudden"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""certain"",
      ""hidden""
    ]
  },
  {
    ""word"": ""suffer"",
    ""rhymes"": [],
    ""soundsLike"": []
  },
  {
    ""word"": ""sugar"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""sure"",
      ""share""
    ]
  },
  {
    ""word"": ""suggest"",
    ""rhymes"": [
      ""test"",
      ""best"",
      ""nest"",
      ""chest"",
      ""west"",
      ""arrest"",
      ""invest""
    ],
    ""soundsLike"": [
      ""success"",
      ""subject""
    ]
  },
  {
    ""word"": ""suit"",
    ""rhymes"": [
      ""shoot"",
      ""route"",
      ""fruit"",
      ""minute"",
      ""execute"",
      ""cute"",
      ""salute"",
      ""lawsuit""
    ],
    ""soundsLike"": [
      ""sight"",
      ""seat"",
      ""sad""
    ]
  },
  {
    ""word"": ""summer"",
    ""rhymes"": [],
    ""soundsLike"": []
  },
  {
    ""word"": ""sun"",
    ""rhymes"": [
      ""run"",
      ""one"",
      ""gun"",
      ""fun"",
      ""someone""
    ],
    ""soundsLike"": [
      ""soon"",
      ""scene"",
      ""sign"",
      ""sing""
    ]
  },
  {
    ""word"": ""sunny"",
    ""rhymes"": [
      ""funny"",
      ""honey""
    ],
    ""soundsLike"": [
      ""sun""
    ]
  },
  {
    ""word"": ""sunset"",
    ""rhymes"": [
      ""net"",
      ""upset"",
      ""asset"",
      ""wet"",
      ""forget"",
      ""regret"",
      ""pet""
    ],
    ""soundsLike"": [
      ""census""
    ]
  },
  {
    ""word"": ""super"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""soup""
    ]
  },
  {
    ""word"": ""supply"",
    ""rhymes"": [
      ""fly"",
      ""eye"",
      ""high"",
      ""identify"",
      ""dry"",
      ""shy"",
      ""cry"",
      ""clarify"",
      ""try"",
      ""verify"",
      ""deny"",
      ""defy"",
      ""rely"",
      ""modify"",
      ""satisfy"",
      ""spy""
    ],
    ""soundsLike"": [
      ""silly""
    ]
  },
  {
    ""word"": ""supreme"",
    ""rhymes"": [
      ""dream"",
      ""cream"",
      ""scheme"",
      ""team"",
      ""theme""
    ],
    ""soundsLike"": [
      ""surprise"",
      ""spring""
    ]
  },
  {
    ""word"": ""sure"",
    ""rhymes"": [
      ""ensure"",
      ""obscure""
    ],
    ""soundsLike"": [
      ""share"",
      ""sugar"",
      ""chair"",
      ""jar""
    ]
  },
  {
    ""word"": ""surface"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""service""
    ]
  },
  {
    ""word"": ""surge"",
    ""rhymes"": [
      ""urge"",
      ""emerge"",
      ""merge""
    ],
    ""soundsLike"": [
      ""search"",
      ""siege""
    ]
  },
  {
    ""word"": ""surprise"",
    ""rhymes"": [
      ""exercise"",
      ""demise"",
      ""wise"",
      ""size"",
      ""prize""
    ],
    ""soundsLike"": [
      ""supreme"",
      ""series""
    ]
  },
  {
    ""word"": ""surround"",
    ""rhymes"": [
      ""around"",
      ""round"",
      ""sound"",
      ""found""
    ],
    ""soundsLike"": [
      ""sound"",
      ""sand"",
      ""around"",
      ""round""
    ]
  },
  {
    ""word"": ""survey"",
    ""rhymes"": [
      ""day"",
      ""away"",
      ""way"",
      ""play"",
      ""say"",
      ""display"",
      ""stay"",
      ""delay"",
      ""holiday"",
      ""essay"",
      ""clay"",
      ""betray"",
      ""spray"",
      ""they"",
      ""okay"",
      ""tray"",
      ""obey"",
      ""today""
    ],
    ""soundsLike"": [
      ""service""
    ]
  },
  {
    ""word"": ""suspect"",
    ""rhymes"": [
      ""project"",
      ""subject"",
      ""aspect"",
      ""object"",
      ""perfect"",
      ""reflect"",
      ""direct"",
      ""connect"",
      ""expect"",
      ""correct"",
      ""neglect"",
      ""collect"",
      ""select"",
      ""protect"",
      ""reject"",
      ""detect"",
      ""insect"",
      ""inject""
    ],
    ""soundsLike"": [
      ""expect"",
      ""aspect"",
      ""subject"",
      ""sunset"",
      ""select""
    ]
  },
  {
    ""word"": ""sustain"",
    ""rhymes"": [
      ""train"",
      ""domain"",
      ""rain"",
      ""again"",
      ""grain"",
      ""obtain"",
      ""brain"",
      ""gain"",
      ""explain"",
      ""main"",
      ""crane"",
      ""insane"",
      ""remain""
    ],
    ""soundsLike"": [
      ""system"",
      ""sudden""
    ]
  },
  {
    ""word"": ""swallow"",
    ""rhymes"": [
      ""follow"",
      ""hollow""
    ],
    ""soundsLike"": []
  },
  {
    ""word"": ""swamp"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""swap"",
      ""swim"",
      ""stamp""
    ]
  },
  {
    ""word"": ""swap"",
    ""rhymes"": [
      ""top"",
      ""shop"",
      ""drop"",
      ""crop"",
      ""laptop""
    ],
    ""soundsLike"": [
      ""swamp"",
      ""sleep"",
      ""snap"",
      ""soap""
    ]
  },
  {
    ""word"": ""swarm"",
    ""rhymes"": [
      ""inform"",
      ""warm"",
      ""uniform"",
      ""reform""
    ],
    ""soundsLike"": [
      ""swim"",
      ""swear"",
      ""story""
    ]
  },
  {
    ""word"": ""swear"",
    ""rhymes"": [
      ""air"",
      ""chair"",
      ""wear"",
      ""square"",
      ""share"",
      ""despair"",
      ""there"",
      ""hair"",
      ""spare"",
      ""aware"",
      ""pair"",
      ""where"",
      ""rare"",
      ""pear"",
      ""affair"",
      ""repair"",
      ""prepare"",
      ""glare"",
      ""scare"",
      ""warfare"",
      ""unfair"",
      ""unaware""
    ],
    ""soundsLike"": [
      ""square"",
      ""spare"",
      ""where"",
      ""aware""
    ]
  },
  {
    ""word"": ""sweet"",
    ""rhymes"": [
      ""seat"",
      ""treat"",
      ""street"",
      ""elite"",
      ""meat"",
      ""retreat"",
      ""athlete"",
      ""wheat"",
      ""repeat""
    ],
    ""soundsLike"": [
      ""seat"",
      ""wheat"",
      ""seed""
    ]
  },
  {
    ""word"": ""swift"",
    ""rhymes"": [
      ""shift"",
      ""lift"",
      ""drift"",
      ""gift""
    ],
    ""soundsLike"": [
      ""sweet"",
      ""soft"",
      ""sniff""
    ]
  },
  {
    ""word"": ""swim"",
    ""rhymes"": [
      ""trim"",
      ""limb"",
      ""slim"",
      ""gym""
    ],
    ""soundsLike"": [
      ""swing"",
      ""slim"",
      ""stem""
    ]
  },
  {
    ""word"": ""swing"",
    ""rhymes"": [
      ""ring"",
      ""bring"",
      ""spring"",
      ""sting"",
      ""thing"",
      ""wing"",
      ""sing""
    ],
    ""soundsLike"": [
      ""swim"",
      ""sing"",
      ""sting"",
      ""song"",
      ""slim""
    ]
  },
  {
    ""word"": ""switch"",
    ""rhymes"": [
      ""pitch"",
      ""rich"",
      ""enrich""
    ],
    ""soundsLike"": [
      ""such"",
      ""sweet""
    ]
  },
  {
    ""word"": ""sword"",
    ""rhymes"": [
      ""board"",
      ""record"",
      ""afford"",
      ""toward"",
      ""reward""
    ],
    ""soundsLike"": [
      ""sort"",
      ""sorry""
    ]
  },
  {
    ""word"": ""symbol"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""simple"",
      ""sample"",
      ""stumble"",
      ""humble""
    ]
  },
  {
    ""word"": ""symptom"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""simple"",
      ""system""
    ]
  },
  {
    ""word"": ""syrup"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""soup"",
      ""soap"",
      ""scrub"",
      ""scrap""
    ]
  },
  {
    ""word"": ""system"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""sustain"",
      ""sister"",
      ""custom""
    ]
  },
  {
    ""word"": ""table"",
    ""rhymes"": [
      ""label"",
      ""enable"",
      ""able"",
      ""stable"",
      ""cable"",
      ""unable""
    ],
    ""soundsLike"": [
      ""stable"",
      ""topple"",
      ""double"",
      ""trouble"",
      ""tumble"",
      ""cable""
    ]
  },
  {
    ""word"": ""tackle"",
    ""rhymes"": [],
    ""soundsLike"": []
  },
  {
    ""word"": ""tag"",
    ""rhymes"": [
      ""bag"",
      ""flag""
    ],
    ""soundsLike"": [
      ""talk""
    ]
  },
  {
    ""word"": ""tail"",
    ""rhymes"": [
      ""scale"",
      ""rail"",
      ""mail"",
      ""whale"",
      ""detail"",
      ""sail"",
      ""female"",
      ""unveil"",
      ""inhale""
    ],
    ""soundsLike"": [
      ""tell"",
      ""tool"",
      ""deal""
    ]
  },
  {
    ""word"": ""talent"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""toilet"",
      ""tenant""
    ]
  },
  {
    ""word"": ""talk"",
    ""rhymes"": [
      ""stock"",
      ""lock"",
      ""clock"",
      ""walk"",
      ""shock"",
      ""hawk"",
      ""flock"",
      ""knock"",
      ""chalk"",
      ""sock"",
      ""unlock""
    ],
    ""soundsLike"": [
      ""tag""
    ]
  },
  {
    ""word"": ""tank"",
    ""rhymes"": [
      ""thank""
    ],
    ""soundsLike"": [
      ""thank"",
      ""talk"",
      ""task"",
      ""tag""
    ]
  },
  {
    ""word"": ""tape"",
    ""rhymes"": [
      ""escape"",
      ""grape""
    ],
    ""soundsLike"": [
      ""top"",
      ""type"",
      ""tip"",
      ""tube""
    ]
  },
  {
    ""word"": ""target"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""ticket"",
      ""market"",
      ""forget""
    ]
  },
  {
    ""word"": ""task"",
    ""rhymes"": [
      ""ask"",
      ""mask""
    ],
    ""soundsLike"": [
      ""desk"",
      ""talk"",
      ""tank"",
      ""tag""
    ]
  },
  {
    ""word"": ""taste"",
    ""rhymes"": [
      ""waste""
    ],
    ""soundsLike"": [
      ""test"",
      ""toast"",
      ""dust"",
      ""waste""
    ]
  },
  {
    ""word"": ""tattoo"",
    ""rhymes"": [
      ""blue"",
      ""into"",
      ""you"",
      ""view"",
      ""review"",
      ""true"",
      ""two"",
      ""shoe"",
      ""crew"",
      ""tissue"",
      ""few"",
      ""glue"",
      ""bamboo"",
      ""undo"",
      ""renew"",
      ""canoe"",
      ""zoo""
    ],
    ""soundsLike"": []
  },
  {
    ""word"": ""taxi"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""text""
    ]
  },
  {
    ""word"": ""teach"",
    ""rhymes"": [
      ""beach""
    ],
    ""soundsLike"": []
  },
  {
    ""word"": ""team"",
    ""rhymes"": [
      ""dream"",
      ""cream"",
      ""scheme"",
      ""theme"",
      ""supreme""
    ],
    ""soundsLike"": [
      ""time"",
      ""ten"",
      ""term""
    ]
  },
  {
    ""word"": ""tell"",
    ""rhymes"": [
      ""spell"",
      ""shell"",
      ""sell"",
      ""rebel"",
      ""hotel""
    ],
    ""soundsLike"": [
      ""tail"",
      ""tool""
    ]
  },
  {
    ""word"": ""ten"",
    ""rhymes"": [
      ""pen"",
      ""then"",
      ""again"",
      ""hen"",
      ""when""
    ],
    ""soundsLike"": [
      ""town"",
      ""tone"",
      ""tongue"",
      ""team""
    ]
  },
  {
    ""word"": ""tenant"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""tonight"",
      ""talent"",
      ""tent""
    ]
  },
  {
    ""word"": ""tennis"",
    ""rhymes"": [],
    ""soundsLike"": []
  },
  {
    ""word"": ""tent"",
    ""rhymes"": [
      ""present"",
      ""rent"",
      ""prevent"",
      ""segment"",
      ""orient"",
      ""cement"",
      ""frequent""
    ],
    ""soundsLike"": [
      ""ten""
    ]
  },
  {
    ""word"": ""term"",
    ""rhymes"": [
      ""confirm"",
      ""firm""
    ],
    ""soundsLike"": [
      ""turn"",
      ""time"",
      ""true"",
      ""tree"",
      ""team"",
      ""try"",
      ""tray"",
      ""trim""
    ]
  },
  {
    ""word"": ""test"",
    ""rhymes"": [
      ""best"",
      ""suggest"",
      ""nest"",
      ""chest"",
      ""west"",
      ""arrest"",
      ""invest""
    ],
    ""soundsLike"": [
      ""taste"",
      ""toast"",
      ""dust"",
      ""text"",
      ""chest"",
      ""west"",
      ""trust"",
      ""twist""
    ]
  },
  {
    ""word"": ""text"",
    ""rhymes"": [
      ""next""
    ],
    ""soundsLike"": [
      ""test"",
      ""ticket"",
      ""taste"",
      ""toast"",
      ""taxi"",
      ""next""
    ]
  },
  {
    ""word"": ""thank"",
    ""rhymes"": [
      ""tank""
    ],
    ""soundsLike"": [
      ""tank"",
      ""thing""
    ]
  },
  {
    ""word"": ""that"",
    ""rhymes"": [
      ""cat"",
      ""nut"",
      ""hat"",
      ""flat"",
      ""what"",
      ""fat"",
      ""chat"",
      ""robot"",
      ""walnut"",
      ""peanut"",
      ""coconut""
    ],
    ""soundsLike"": [
      ""thought"",
      ""fat"",
      ""hat"",
      ""chat""
    ]
  },
  {
    ""word"": ""theme"",
    ""rhymes"": [
      ""dream"",
      ""cream"",
      ""scheme"",
      ""team"",
      ""supreme""
    ],
    ""soundsLike"": [
      ""thumb"",
      ""thing"",
      ""then"",
      ""team""
    ]
  },
  {
    ""word"": ""then"",
    ""rhymes"": [
      ""pen"",
      ""again"",
      ""hen"",
      ""when"",
      ""ten""
    ],
    ""soundsLike"": [
      ""thing"",
      ""thumb"",
      ""theme"",
      ""hen"",
      ""when""
    ]
  },
  {
    ""word"": ""theory"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""three"",
      ""there"",
      ""carry""
    ]
  },
  {
    ""word"": ""there"",
    ""rhymes"": [
      ""air"",
      ""chair"",
      ""wear"",
      ""square"",
      ""share"",
      ""despair"",
      ""hair"",
      ""spare"",
      ""aware"",
      ""pair"",
      ""where"",
      ""rare"",
      ""swear"",
      ""pear"",
      ""affair"",
      ""repair"",
      ""prepare"",
      ""glare"",
      ""scare"",
      ""warfare"",
      ""unfair"",
      ""unaware""
    ],
    ""soundsLike"": [
      ""hair"",
      ""chair"",
      ""rare""
    ]
  },
  {
    ""word"": ""they"",
    ""rhymes"": [
      ""day"",
      ""away"",
      ""way"",
      ""play"",
      ""say"",
      ""survey"",
      ""display"",
      ""stay"",
      ""delay"",
      ""holiday"",
      ""essay"",
      ""clay"",
      ""betray"",
      ""spray"",
      ""okay"",
      ""tray"",
      ""obey"",
      ""today""
    ],
    ""soundsLike"": [
      ""day""
    ]
  },
  {
    ""word"": ""thing"",
    ""rhymes"": [
      ""ring"",
      ""bring"",
      ""spring"",
      ""swing"",
      ""sting"",
      ""wing"",
      ""sing""
    ],
    ""soundsLike"": [
      ""then"",
      ""thumb"",
      ""theme"",
      ""sing"",
      ""thank""
    ]
  },
  {
    ""word"": ""this"",
    ""rhymes"": [
      ""miss"",
      ""dismiss"",
      ""kiss""
    ],
    ""soundsLike"": [
      ""miss""
    ]
  },
  {
    ""word"": ""thought"",
    ""rhymes"": [
      ""spot"",
      ""slot"",
      ""robot"",
      ""caught""
    ],
    ""soundsLike"": [
      ""that"",
      ""caught""
    ]
  },
  {
    ""word"": ""three"",
    ""rhymes"": [
      ""tree"",
      ""sea"",
      ""key"",
      ""flee"",
      ""degree"",
      ""debris"",
      ""agree"",
      ""fee"",
      ""ski"",
      ""pony"",
      ""knee""
    ],
    ""soundsLike"": [
      ""throw"",
      ""theory""
    ]
  },
  {
    ""word"": ""thrive"",
    ""rhymes"": [
      ""drive"",
      ""live"",
      ""derive"",
      ""arrive""
    ],
    ""soundsLike"": [
      ""three"",
      ""throw"",
      ""drive"",
      ""derive"",
      ""arrive""
    ]
  },
  {
    ""word"": ""throw"",
    ""rhymes"": [
      ""know"",
      ""snow"",
      ""hello"",
      ""grow"",
      ""shadow"",
      ""potato"",
      ""toe"",
      ""glow"",
      ""slow"",
      ""tomato"",
      ""below"",
      ""radio"",
      ""meadow"",
      ""tomorrow"",
      ""borrow"",
      ""photo"",
      ""tornado"",
      ""buffalo""
    ],
    ""soundsLike"": [
      ""three"",
      ""arrow""
    ]
  },
  {
    ""word"": ""thumb"",
    ""rhymes"": [
      ""become"",
      ""come"",
      ""income"",
      ""drum"",
      ""dumb""
    ],
    ""soundsLike"": [
      ""theme"",
      ""thing"",
      ""then"",
      ""come""
    ]
  },
  {
    ""word"": ""thunder"",
    ""rhymes"": [
      ""wonder"",
      ""under""
    ],
    ""soundsLike"": [
      ""wonder"",
      ""render""
    ]
  },
  {
    ""word"": ""ticket"",
    ""rhymes"": [
      ""cricket""
    ],
    ""soundsLike"": [
      ""text"",
      ""target""
    ]
  },
  {
    ""word"": ""tide"",
    ""rhymes"": [
      ""side"",
      ""provide"",
      ""ride"",
      ""slide"",
      ""pride"",
      ""guide"",
      ""decide"",
      ""divide"",
      ""wide"",
      ""glide"",
      ""inside"",
      ""outside""
    ],
    ""soundsLike"": [
      ""tired""
    ]
  },
  {
    ""word"": ""tiger"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""tag""
    ]
  },
  {
    ""word"": ""tilt"",
    ""rhymes"": [
      ""guilt""
    ],
    ""soundsLike"": [
      ""tell""
    ]
  },
  {
    ""word"": ""timber"",
    ""rhymes"": [],
    ""soundsLike"": []
  },
  {
    ""word"": ""time"",
    ""rhymes"": [
      ""crime"",
      ""climb""
    ],
    ""soundsLike"": [
      ""team"",
      ""ten"",
      ""tone"",
      ""term""
    ]
  },
  {
    ""word"": ""tiny"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""tuna""
    ]
  },
  {
    ""word"": ""tip"",
    ""rhymes"": [
      ""ship"",
      ""trip"",
      ""clip"",
      ""flip"",
      ""whip"",
      ""drip"",
      ""hip"",
      ""equip""
    ],
    ""soundsLike"": [
      ""top"",
      ""type"",
      ""tape"",
      ""trip"",
      ""tube"",
      ""hip""
    ]
  },
  {
    ""word"": ""tired"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""toward"",
      ""tide""
    ]
  },
  {
    ""word"": ""tissue"",
    ""rhymes"": [
      ""blue"",
      ""into"",
      ""you"",
      ""issue"",
      ""view"",
      ""review"",
      ""true"",
      ""two"",
      ""shoe"",
      ""crew"",
      ""few"",
      ""glue"",
      ""bamboo"",
      ""undo"",
      ""tattoo"",
      ""renew"",
      ""canoe"",
      ""zoo""
    ],
    ""soundsLike"": []
  },
  {
    ""word"": ""title"",
    ""rhymes"": [
      ""vital""
    ],
    ""soundsLike"": [
      ""total"",
      ""turtle""
    ]
  },
  {
    ""word"": ""toast"",
    ""rhymes"": [
      ""post"",
      ""host"",
      ""coast"",
      ""roast"",
      ""ghost"",
      ""almost""
    ],
    ""soundsLike"": [
      ""test"",
      ""taste"",
      ""host"",
      ""dust""
    ]
  },
  {
    ""word"": ""tobacco"",
    ""rhymes"": [
      ""know"",
      ""snow"",
      ""throw"",
      ""hello"",
      ""grow"",
      ""shadow"",
      ""potato"",
      ""toe"",
      ""glow"",
      ""slow"",
      ""tomato"",
      ""below"",
      ""radio"",
      ""meadow"",
      ""tomorrow"",
      ""borrow"",
      ""photo"",
      ""tornado"",
      ""buffalo""
    ],
    ""soundsLike"": []
  },
  {
    ""word"": ""today"",
    ""rhymes"": [
      ""day"",
      ""away"",
      ""way"",
      ""play"",
      ""say"",
      ""survey"",
      ""display"",
      ""stay"",
      ""delay"",
      ""holiday"",
      ""essay"",
      ""clay"",
      ""betray"",
      ""spray"",
      ""they"",
      ""okay"",
      ""tray"",
      ""obey""
    ],
    ""soundsLike"": []
  },
  {
    ""word"": ""toddler"",
    ""rhymes"": [],
    ""soundsLike"": []
  },
  {
    ""word"": ""toe"",
    ""rhymes"": [
      ""know"",
      ""snow"",
      ""throw"",
      ""hello"",
      ""grow"",
      ""shadow"",
      ""potato"",
      ""glow"",
      ""slow"",
      ""tomato"",
      ""below"",
      ""radio"",
      ""meadow"",
      ""tomorrow"",
      ""borrow"",
      ""photo"",
      ""tornado"",
      ""buffalo""
    ],
    ""soundsLike"": [
      ""two"",
      ""toy"",
      ""auto""
    ]
  },
  {
    ""word"": ""together"",
    ""rhymes"": [
      ""weather""
    ],
    ""soundsLike"": [
      ""tiger""
    ]
  },
  {
    ""word"": ""toilet"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""talent"",
      ""tilt""
    ]
  },
  {
    ""word"": ""token"",
    ""rhymes"": [
      ""broken""
    ],
    ""soundsLike"": []
  },
  {
    ""word"": ""tomato"",
    ""rhymes"": [
      ""know"",
      ""snow"",
      ""throw"",
      ""hello"",
      ""grow"",
      ""shadow"",
      ""potato"",
      ""toe"",
      ""glow"",
      ""slow"",
      ""below"",
      ""radio"",
      ""meadow"",
      ""tomorrow"",
      ""borrow"",
      ""photo"",
      ""tornado"",
      ""buffalo""
    ],
    ""soundsLike"": [
      ""tomorrow"",
      ""tonight""
    ]
  },
  {
    ""word"": ""tomorrow"",
    ""rhymes"": [
      ""know"",
      ""snow"",
      ""throw"",
      ""hello"",
      ""grow"",
      ""shadow"",
      ""potato"",
      ""toe"",
      ""glow"",
      ""slow"",
      ""tomato"",
      ""below"",
      ""radio"",
      ""meadow"",
      ""borrow"",
      ""photo"",
      ""tornado"",
      ""buffalo""
    ],
    ""soundsLike"": [
      ""tomato""
    ]
  },
  {
    ""word"": ""tone"",
    ""rhymes"": [
      ""bone"",
      ""zone"",
      ""stone"",
      ""loan"",
      ""phone"",
      ""own"",
      ""alone"",
      ""ozone"",
      ""unknown""
    ],
    ""soundsLike"": [
      ""town"",
      ""ten"",
      ""time"",
      ""team"",
      ""stone"",
      ""toe""
    ]
  },
  {
    ""word"": ""tongue"",
    ""rhymes"": [
      ""among"",
      ""young""
    ],
    ""soundsLike"": [
      ""ten"",
      ""time"",
      ""town"",
      ""team"",
      ""tone""
    ]
  },
  {
    ""word"": ""tonight"",
    ""rhymes"": [
      ""light"",
      ""right"",
      ""night"",
      ""write"",
      ""sight"",
      ""flight"",
      ""bright"",
      ""slight"",
      ""height"",
      ""kite"",
      ""invite"",
      ""excite"",
      ""midnight""
    ],
    ""soundsLike"": [
      ""tent"",
      ""donate"",
      ""tenant""
    ]
  },
  {
    ""word"": ""tool"",
    ""rhymes"": [
      ""school"",
      ""rule"",
      ""pool"",
      ""cool"",
      ""fuel"",
      ""stool"",
      ""cruel"",
      ""mule"",
      ""jewel""
    ],
    ""soundsLike"": [
      ""tell"",
      ""tail"",
      ""two"",
      ""stool""
    ]
  },
  {
    ""word"": ""tooth"",
    ""rhymes"": [
      ""truth"",
      ""youth""
    ],
    ""soundsLike"": [
      ""two"",
      ""truth"",
      ""youth""
    ]
  },
  {
    ""word"": ""top"",
    ""rhymes"": [
      ""shop"",
      ""drop"",
      ""crop"",
      ""swap"",
      ""laptop""
    ],
    ""soundsLike"": [
      ""type"",
      ""tape"",
      ""tip"",
      ""tube""
    ]
  },
  {
    ""word"": ""topic"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""topple""
    ]
  },
  {
    ""word"": ""topple"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""table"",
      ""double""
    ]
  },
  {
    ""word"": ""torch"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""tower""
    ]
  },
  {
    ""word"": ""tornado"",
    ""rhymes"": [
      ""know"",
      ""snow"",
      ""throw"",
      ""hello"",
      ""grow"",
      ""shadow"",
      ""potato"",
      ""toe"",
      ""glow"",
      ""slow"",
      ""tomato"",
      ""below"",
      ""radio"",
      ""meadow"",
      ""tomorrow"",
      ""borrow"",
      ""photo"",
      ""buffalo""
    ],
    ""soundsLike"": [
      ""tomato""
    ]
  },
  {
    ""word"": ""tortoise"",
    ""rhymes"": [],
    ""soundsLike"": []
  },
  {
    ""word"": ""toss"",
    ""rhymes"": [
      ""cross"",
      ""sauce"",
      ""across"",
      ""boss""
    ],
    ""soundsLike"": []
  },
  {
    ""word"": ""total"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""title"",
      ""hotel"",
      ""turtle"",
      ""detail""
    ]
  },
  {
    ""word"": ""tourist"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""trust""
    ]
  },
  {
    ""word"": ""toward"",
    ""rhymes"": [
      ""board"",
      ""record"",
      ""afford"",
      ""reward"",
      ""sword""
    ],
    ""soundsLike"": [
      ""reward"",
      ""tired""
    ]
  },
  {
    ""word"": ""tower"",
    ""rhymes"": [
      ""power"",
      ""flower"",
      ""hour"",
      ""empower""
    ],
    ""soundsLike"": [
      ""door"",
      ""deer""
    ]
  },
  {
    ""word"": ""town"",
    ""rhymes"": [
      ""around"",
      ""brown"",
      ""frown"",
      ""gown"",
      ""clown""
    ],
    ""soundsLike"": [
      ""ten"",
      ""tone"",
      ""time"",
      ""team"",
      ""dawn""
    ]
  },
  {
    ""word"": ""toy"",
    ""rhymes"": [
      ""employ"",
      ""boy"",
      ""joy"",
      ""enjoy"",
      ""destroy""
    ],
    ""soundsLike"": [
      ""two"",
      ""toe""
    ]
  },
  {
    ""word"": ""track"",
    ""rhymes"": [
      ""black"",
      ""attack"",
      ""crack"",
      ""rack"",
      ""snack""
    ],
    ""soundsLike"": [
      ""truck"",
      ""trick"",
      ""strike""
    ]
  },
  {
    ""word"": ""trade"",
    ""rhymes"": [
      ""blade"",
      ""parade"",
      ""fade"",
      ""afraid"",
      ""decade"",
      ""upgrade"",
      ""maid""
    ],
    ""soundsLike"": [
      ""treat"",
      ""tray""
    ]
  },
  {
    ""word"": ""traffic"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""tragic"",
      ""track"",
      ""trick"",
      ""trophy"",
      ""truck""
    ]
  },
  {
    ""word"": ""tragic"",
    ""rhymes"": [
      ""magic""
    ],
    ""soundsLike"": [
      ""traffic"",
      ""track"",
      ""trick"",
      ""truck"",
      ""trash""
    ]
  },
  {
    ""word"": ""train"",
    ""rhymes"": [
      ""domain"",
      ""sustain"",
      ""rain"",
      ""again"",
      ""grain"",
      ""obtain"",
      ""brain"",
      ""gain"",
      ""explain"",
      ""main"",
      ""crane"",
      ""insane"",
      ""remain""
    ],
    ""soundsLike"": [
      ""turn"",
      ""tray"",
      ""trim""
    ]
  },
  {
    ""word"": ""transfer"",
    ""rhymes"": [
      ""occur"",
      ""amateur"",
      ""prefer"",
      ""blur""
    ],
    ""soundsLike"": []
  },
  {
    ""word"": ""trap"",
    ""rhymes"": [
      ""snap"",
      ""gap"",
      ""wrap"",
      ""scrap"",
      ""clap""
    ],
    ""soundsLike"": [
      ""trip"",
      ""tribe"",
      ""drop"",
      ""drip"",
      ""top"",
      ""tape"",
      ""tip""
    ]
  },
  {
    ""word"": ""trash"",
    ""rhymes"": [
      ""dash"",
      ""flash"",
      ""crash"",
      ""cash""
    ],
    ""soundsLike"": [
      ""tree"",
      ""track"",
      ""treat"",
      ""trap"",
      ""tray""
    ]
  },
  {
    ""word"": ""travel"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""trial"",
      ""turtle""
    ]
  },
  {
    ""word"": ""tray"",
    ""rhymes"": [
      ""day"",
      ""away"",
      ""way"",
      ""play"",
      ""say"",
      ""survey"",
      ""display"",
      ""stay"",
      ""delay"",
      ""holiday"",
      ""essay"",
      ""clay"",
      ""betray"",
      ""spray"",
      ""they"",
      ""okay"",
      ""obey"",
      ""today""
    ],
    ""soundsLike"": [
      ""true"",
      ""tree"",
      ""try"",
      ""turn"",
      ""train"",
      ""trade"",
      ""term""
    ]
  },
  {
    ""word"": ""treat"",
    ""rhymes"": [
      ""seat"",
      ""street"",
      ""elite"",
      ""sweet"",
      ""meat"",
      ""retreat"",
      ""athlete"",
      ""wheat"",
      ""repeat""
    ],
    ""soundsLike"": [
      ""tree"",
      ""street"",
      ""trade""
    ]
  },
  {
    ""word"": ""tree"",
    ""rhymes"": [
      ""sea"",
      ""key"",
      ""flee"",
      ""degree"",
      ""debris"",
      ""agree"",
      ""fee"",
      ""ski"",
      ""three"",
      ""pony"",
      ""knee""
    ],
    ""soundsLike"": [
      ""true"",
      ""try"",
      ""tray"",
      ""term""
    ]
  },
  {
    ""word"": ""trend"",
    ""rhymes"": [
      ""end"",
      ""friend"",
      ""lend"",
      ""attend"",
      ""extend"",
      ""spend"",
      ""depend"",
      ""weekend""
    ],
    ""soundsLike"": [
      ""train"",
      ""tent"",
      ""trade""
    ]
  },
  {
    ""word"": ""trial"",
    ""rhymes"": [
      ""style"",
      ""file"",
      ""aisle"",
      ""smile"",
      ""exile"",
      ""denial"",
      ""dial""
    ],
    ""soundsLike"": [
      ""turtle"",
      ""trouble"",
      ""travel"",
      ""dial""
    ]
  },
  {
    ""word"": ""tribe"",
    ""rhymes"": [
      ""describe""
    ],
    ""soundsLike"": [
      ""try"",
      ""trip"",
      ""trap"",
      ""tree"",
      ""tray""
    ]
  },
  {
    ""word"": ""trick"",
    ""rhymes"": [
      ""stick"",
      ""kick"",
      ""quick"",
      ""sick"",
      ""click"",
      ""brick"",
      ""picnic""
    ],
    ""soundsLike"": [
      ""truck"",
      ""track""
    ]
  },
  {
    ""word"": ""trigger"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""trick"",
      ""tiger"",
      ""truck""
    ]
  },
  {
    ""word"": ""trim"",
    ""rhymes"": [
      ""swim"",
      ""limb"",
      ""slim"",
      ""gym""
    ],
    ""soundsLike"": [
      ""train"",
      ""drum"",
      ""term"",
      ""dream"",
      ""during""
    ]
  },
  {
    ""word"": ""trip"",
    ""rhymes"": [
      ""ship"",
      ""tip"",
      ""clip"",
      ""flip"",
      ""whip"",
      ""drip"",
      ""hip"",
      ""equip""
    ],
    ""soundsLike"": [
      ""trap"",
      ""drip"",
      ""tip"",
      ""tribe"",
      ""drop"",
      ""tape""
    ]
  },
  {
    ""word"": ""trophy"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""tree"",
      ""traffic""
    ]
  },
  {
    ""word"": ""trouble"",
    ""rhymes"": [
      ""bubble"",
      ""double""
    ],
    ""soundsLike"": [
      ""table"",
      ""trial"",
      ""double""
    ]
  },
  {
    ""word"": ""truck"",
    ""rhymes"": [
      ""duck"",
      ""pluck""
    ],
    ""soundsLike"": [
      ""trick"",
      ""track""
    ]
  },
  {
    ""word"": ""true"",
    ""rhymes"": [
      ""blue"",
      ""into"",
      ""you"",
      ""view"",
      ""review"",
      ""two"",
      ""shoe"",
      ""crew"",
      ""tissue"",
      ""few"",
      ""glue"",
      ""bamboo"",
      ""undo"",
      ""tattoo"",
      ""renew"",
      ""canoe"",
      ""zoo""
    ],
    ""soundsLike"": [
      ""tree"",
      ""try"",
      ""tray"",
      ""truth"",
      ""turn"",
      ""term""
    ]
  },
  {
    ""word"": ""truly"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""trial""
    ]
  },
  {
    ""word"": ""trumpet"",
    ""rhymes"": [],
    ""soundsLike"": []
  },
  {
    ""word"": ""trust"",
    ""rhymes"": [
      ""robust"",
      ""just"",
      ""dust"",
      ""must"",
      ""adjust""
    ],
    ""soundsLike"": [
      ""tourist"",
      ""test"",
      ""twist"",
      ""taste"",
      ""toast""
    ]
  },
  {
    ""word"": ""truth"",
    ""rhymes"": [
      ""tooth"",
      ""youth""
    ],
    ""soundsLike"": [
      ""true"",
      ""tooth"",
      ""tree"",
      ""track"",
      ""treat"",
      ""tray""
    ]
  },
  {
    ""word"": ""try"",
    ""rhymes"": [
      ""fly"",
      ""eye"",
      ""high"",
      ""supply"",
      ""identify"",
      ""dry"",
      ""shy"",
      ""cry"",
      ""clarify"",
      ""verify"",
      ""deny"",
      ""defy"",
      ""rely"",
      ""modify"",
      ""satisfy"",
      ""spy""
    ],
    ""soundsLike"": [
      ""true"",
      ""tree"",
      ""tray"",
      ""dry"",
      ""turn"",
      ""trial"",
      ""term"",
      ""tribe""
    ]
  },
  {
    ""word"": ""tube"",
    ""rhymes"": [
      ""cube""
    ],
    ""soundsLike"": [
      ""two"",
      ""top"",
      ""type"",
      ""tape"",
      ""tip""
    ]
  },
  {
    ""word"": ""tuition"",
    ""rhymes"": [
      ""position""
    ],
    ""soundsLike"": []
  },
  {
    ""word"": ""tumble"",
    ""rhymes"": [
      ""humble"",
      ""stumble"",
      ""crumble""
    ],
    ""soundsLike"": [
      ""stumble"",
      ""humble"",
      ""table"",
      ""tunnel"",
      ""double""
    ]
  },
  {
    ""word"": ""tuna"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""tiny""
    ]
  },
  {
    ""word"": ""tunnel"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""tuna"",
      ""tumble""
    ]
  },
  {
    ""word"": ""turkey"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""tree"",
      ""tray""
    ]
  },
  {
    ""word"": ""turn"",
    ""rhymes"": [
      ""learn"",
      ""return"",
      ""churn"",
      ""earn""
    ],
    ""soundsLike"": [
      ""term"",
      ""try"",
      ""tree"",
      ""ten"",
      ""train"",
      ""tray""
    ]
  },
  {
    ""word"": ""turtle"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""title"",
      ""trial"",
      ""total"",
      ""hurdle"",
      ""drill""
    ]
  },
  {
    ""word"": ""twelve"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""tell""
    ]
  },
  {
    ""word"": ""twenty"",
    ""rhymes"": [
      ""any""
    ],
    ""soundsLike"": [
      ""tent""
    ]
  },
  {
    ""word"": ""twice"",
    ""rhymes"": [
      ""ice"",
      ""advice"",
      ""device"",
      ""nice"",
      ""price"",
      ""rice"",
      ""dice"",
      ""spice"",
      ""slice""
    ],
    ""soundsLike"": [
      ""twist"",
      ""toss""
    ]
  },
  {
    ""word"": ""twin"",
    ""rhymes"": [
      ""begin"",
      ""spin"",
      ""skin"",
      ""win"",
      ""when"",
      ""violin""
    ],
    ""soundsLike"": [
      ""when"",
      ""ten"",
      ""twenty"",
      ""one""
    ]
  },
  {
    ""word"": ""twist"",
    ""rhymes"": [
      ""list"",
      ""just"",
      ""assist"",
      ""exist"",
      ""resist"",
      ""enlist"",
      ""wrist""
    ],
    ""soundsLike"": [
      ""trust"",
      ""test"",
      ""taste"",
      ""twice"",
      ""toast"",
      ""dust"",
      ""text"",
      ""tourist""
    ]
  },
  {
    ""word"": ""two"",
    ""rhymes"": [
      ""blue"",
      ""into"",
      ""you"",
      ""view"",
      ""review"",
      ""true"",
      ""shoe"",
      ""crew"",
      ""tissue"",
      ""few"",
      ""glue"",
      ""bamboo"",
      ""undo"",
      ""tattoo"",
      ""renew"",
      ""canoe"",
      ""zoo""
    ],
    ""soundsLike"": [
      ""toy"",
      ""toe""
    ]
  },
  {
    ""word"": ""type"",
    ""rhymes"": [
      ""pipe""
    ],
    ""soundsLike"": [
      ""top"",
      ""tape"",
      ""tip"",
      ""tube""
    ]
  },
  {
    ""word"": ""typical"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""tackle"",
      ""topple"",
      ""topic"",
      ""tobacco""
    ]
  },
  {
    ""word"": ""ugly"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""agree"",
      ""allow""
    ]
  },
  {
    ""word"": ""umbrella"",
    ""rhymes"": [],
    ""soundsLike"": []
  },
  {
    ""word"": ""unable"",
    ""rhymes"": [
      ""table"",
      ""label"",
      ""enable"",
      ""able"",
      ""stable"",
      ""cable""
    ],
    ""soundsLike"": [
      ""enable"",
      ""label"",
      ""cable""
    ]
  },
  {
    ""word"": ""unaware"",
    ""rhymes"": [
      ""air"",
      ""chair"",
      ""wear"",
      ""square"",
      ""share"",
      ""despair"",
      ""there"",
      ""hair"",
      ""spare"",
      ""aware"",
      ""pair"",
      ""where"",
      ""rare"",
      ""swear"",
      ""pear"",
      ""affair"",
      ""repair"",
      ""prepare"",
      ""glare"",
      ""scare"",
      ""warfare"",
      ""unfair""
    ],
    ""soundsLike"": [
      ""another"",
      ""aware"",
      ""unfair"",
      ""acquire""
    ]
  },
  {
    ""word"": ""uncle"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""ankle"",
      ""angle"",
      ""jungle""
    ]
  },
  {
    ""word"": ""uncover"",
    ""rhymes"": [
      ""cover"",
      ""hover"",
      ""discover""
    ],
    ""soundsLike"": [
      ""another""
    ]
  },
  {
    ""word"": ""under"",
    ""rhymes"": [
      ""wonder"",
      ""thunder""
    ],
    ""soundsLike"": [
      ""wonder"",
      ""thunder"",
      ""enter"",
      ""end""
    ]
  },
  {
    ""word"": ""undo"",
    ""rhymes"": [
      ""blue"",
      ""into"",
      ""you"",
      ""view"",
      ""review"",
      ""true"",
      ""two"",
      ""shoe"",
      ""crew"",
      ""tissue"",
      ""few"",
      ""glue"",
      ""bamboo"",
      ""tattoo"",
      ""renew"",
      ""canoe"",
      ""zoo""
    ],
    ""soundsLike"": [
      ""into"",
      ""end""
    ]
  },
  {
    ""word"": ""unfair"",
    ""rhymes"": [
      ""air"",
      ""chair"",
      ""wear"",
      ""square"",
      ""share"",
      ""despair"",
      ""there"",
      ""hair"",
      ""spare"",
      ""aware"",
      ""pair"",
      ""where"",
      ""rare"",
      ""swear"",
      ""pear"",
      ""affair"",
      ""repair"",
      ""prepare"",
      ""glare"",
      ""scare"",
      ""warfare"",
      ""unaware""
    ],
    ""soundsLike"": [
      ""affair"",
      ""aware"",
      ""inform"",
      ""enforce""
    ]
  },
  {
    ""word"": ""unfold"",
    ""rhymes"": [
      ""hold"",
      ""gold"",
      ""fold"",
      ""old"",
      ""uphold""
    ],
    ""soundsLike"": []
  },
  {
    ""word"": ""unhappy"",
    ""rhymes"": [
      ""happy""
    ],
    ""soundsLike"": [
      ""adapt""
    ]
  },
  {
    ""word"": ""uniform"",
    ""rhymes"": [
      ""inform"",
      ""warm"",
      ""swarm"",
      ""reform""
    ],
    ""soundsLike"": []
  },
  {
    ""word"": ""unique"",
    ""rhymes"": [
      ""seek"",
      ""bleak"",
      ""speak"",
      ""creek"",
      ""antique""
    ],
    ""soundsLike"": []
  },
  {
    ""word"": ""unit"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""minute""
    ]
  },
  {
    ""word"": ""universe"",
    ""rhymes"": [
      ""nurse"",
      ""purse""
    ],
    ""soundsLike"": []
  },
  {
    ""word"": ""unknown"",
    ""rhymes"": [
      ""bone"",
      ""zone"",
      ""stone"",
      ""tone"",
      ""loan"",
      ""phone"",
      ""own"",
      ""alone"",
      ""ozone""
    ],
    ""soundsLike"": [
      ""onion""
    ]
  },
  {
    ""word"": ""unlock"",
    ""rhymes"": [
      ""stock"",
      ""talk"",
      ""lock"",
      ""clock"",
      ""walk"",
      ""shock"",
      ""hawk"",
      ""flock"",
      ""knock"",
      ""chalk"",
      ""sock""
    ],
    ""soundsLike"": []
  },
  {
    ""word"": ""until"",
    ""rhymes"": [
      ""will"",
      ""still"",
      ""drill"",
      ""hill"",
      ""skill"",
      ""pill"",
      ""ill""
    ],
    ""soundsLike"": [
      ""gentle"",
      ""bundle""
    ]
  },
  {
    ""word"": ""unusual"",
    ""rhymes"": [
      ""fuel"",
      ""cruel"",
      ""jewel""
    ],
    ""soundsLike"": [
      ""mutual"",
      ""annual""
    ]
  },
  {
    ""word"": ""unveil"",
    ""rhymes"": [
      ""scale"",
      ""rail"",
      ""mail"",
      ""whale"",
      ""detail"",
      ""tail"",
      ""sail"",
      ""female"",
      ""inhale""
    ],
    ""soundsLike"": [
      ""until"",
      ""inhale"",
      ""involve""
    ]
  },
  {
    ""word"": ""update"",
    ""rhymes"": [
      ""state"",
      ""rate"",
      ""plate"",
      ""great"",
      ""mandate"",
      ""estate"",
      ""gate"",
      ""indicate"",
      ""wait"",
      ""debate""
    ],
    ""soundsLike"": [
      ""about"",
      ""upset"",
      ""edit"",
      ""audit""
    ]
  },
  {
    ""word"": ""upgrade"",
    ""rhymes"": [
      ""trade"",
      ""blade"",
      ""parade"",
      ""fade"",
      ""afraid"",
      ""decade"",
      ""maid""
    ],
    ""soundsLike"": [
      ""apart"",
      ""afraid"",
      ""agree""
    ]
  },
  {
    ""word"": ""uphold"",
    ""rhymes"": [
      ""hold"",
      ""gold"",
      ""fold"",
      ""old"",
      ""unfold""
    ],
    ""soundsLike"": [
      ""ahead""
    ]
  },
  {
    ""word"": ""upon"",
    ""rhymes"": [
      ""dawn"",
      ""spawn"",
      ""salon"",
      ""lawn""
    ],
    ""soundsLike"": [
      ""open"",
      ""spawn""
    ]
  },
  {
    ""word"": ""upper"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""appear"",
      ""air""
    ]
  },
  {
    ""word"": ""upset"",
    ""rhymes"": [
      ""net"",
      ""asset"",
      ""wet"",
      ""forget"",
      ""regret"",
      ""pet"",
      ""sunset""
    ],
    ""soundsLike"": [
      ""asset"",
      ""about"",
      ""apart"",
      ""absurd""
    ]
  },
  {
    ""word"": ""urban"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""run"",
      ""ribbon"",
      ""around"",
      ""rain""
    ]
  },
  {
    ""word"": ""urge"",
    ""rhymes"": [
      ""surge"",
      ""emerge"",
      ""merge""
    ],
    ""soundsLike"": [
      ""raw"",
      ""ridge"",
      ""merge""
    ]
  },
  {
    ""word"": ""usage"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""message"",
      ""sausage"",
      ""use"",
      ""huge""
    ]
  },
  {
    ""word"": ""use"",
    ""rhymes"": [
      ""abuse"",
      ""produce"",
      ""goose"",
      ""juice"",
      ""reduce"",
      ""excuse""
    ],
    ""soundsLike"": [
      ""you""
    ]
  },
  {
    ""word"": ""used"",
    ""rhymes"": [
      ""amused""
    ],
    ""soundsLike"": [
      ""use"",
      ""yard""
    ]
  },
  {
    ""word"": ""useful"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""usual"",
      ""usage""
    ]
  },
  {
    ""word"": ""useless"",
    ""rhymes"": [],
    ""soundsLike"": []
  },
  {
    ""word"": ""usual"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""visual"",
      ""casual"",
      ""actual"",
      ""unusual"",
      ""mutual""
    ]
  },
  {
    ""word"": ""utility"",
    ""rhymes"": [
      ""ability""
    ],
    ""soundsLike"": []
  },
  {
    ""word"": ""vacant"",
    ""rhymes"": [],
    ""soundsLike"": []
  },
  {
    ""word"": ""vacuum"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""volume""
    ]
  },
  {
    ""word"": ""vague"",
    ""rhymes"": [],
    ""soundsLike"": []
  },
  {
    ""word"": ""valid"",
    ""rhymes"": [
      ""salad""
    ],
    ""soundsLike"": [
      ""valley"",
      ""salad""
    ]
  },
  {
    ""word"": ""valley"",
    ""rhymes"": [
      ""rally"",
      ""alley""
    ],
    ""soundsLike"": [
      ""valve"",
      ""valid""
    ]
  },
  {
    ""word"": ""valve"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""valley"",
      ""evolve""
    ]
  },
  {
    ""word"": ""van"",
    ""rhymes"": [
      ""man"",
      ""can"",
      ""fan"",
      ""scan""
    ],
    ""soundsLike"": [
      ""fan""
    ]
  },
  {
    ""word"": ""vanish"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""finish"",
      ""van""
    ]
  },
  {
    ""word"": ""vapor"",
    ""rhymes"": [
      ""paper""
    ],
    ""soundsLike"": [
      ""paper""
    ]
  },
  {
    ""word"": ""various"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""virus"",
      ""very"",
      ""vicious""
    ]
  },
  {
    ""word"": ""vast"",
    ""rhymes"": [
      ""blast""
    ],
    ""soundsLike"": []
  },
  {
    ""word"": ""vault"",
    ""rhymes"": [
      ""salt"",
      ""fault"",
      ""assault""
    ],
    ""soundsLike"": [
      ""fault"",
      ""valley""
    ]
  },
  {
    ""word"": ""vehicle"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""vocal"",
      ""physical""
    ]
  },
  {
    ""word"": ""velvet"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""vivid"",
      ""valid"",
      ""vault"",
      ""valve""
    ]
  },
  {
    ""word"": ""vendor"",
    ""rhymes"": [
      ""render"",
      ""slender""
    ],
    ""soundsLike"": [
      ""wonder"",
      ""venture"",
      ""render""
    ]
  },
  {
    ""word"": ""venture"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""vendor""
    ]
  },
  {
    ""word"": ""venue"",
    ""rhymes"": [
      ""menu""
    ],
    ""soundsLike"": [
      ""menu""
    ]
  },
  {
    ""word"": ""verb"",
    ""rhymes"": [],
    ""soundsLike"": []
  },
  {
    ""word"": ""verify"",
    ""rhymes"": [
      ""fly"",
      ""eye"",
      ""high"",
      ""supply"",
      ""identify"",
      ""dry"",
      ""shy"",
      ""cry"",
      ""clarify"",
      ""try"",
      ""deny"",
      ""defy"",
      ""rely"",
      ""modify"",
      ""satisfy"",
      ""spy""
    ],
    ""soundsLike"": [
      ""very"",
      ""virus"",
      ""clarify""
    ]
  },
  {
    ""word"": ""version"",
    ""rhymes"": [],
    ""soundsLike"": []
  },
  {
    ""word"": ""very"",
    ""rhymes"": [
      ""cherry"",
      ""carry"",
      ""library"",
      ""primary"",
      ""ordinary"",
      ""merry"",
      ""february""
    ],
    ""soundsLike"": [
      ""cherry""
    ]
  },
  {
    ""word"": ""vessel"",
    ""rhymes"": [
      ""wrestle""
    ],
    ""soundsLike"": [
      ""wrestle"",
      ""fossil""
    ]
  },
  {
    ""word"": ""veteran"",
    ""rhymes"": [],
    ""soundsLike"": []
  },
  {
    ""word"": ""viable"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""vital""
    ]
  },
  {
    ""word"": ""vibrant"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""vacant""
    ]
  },
  {
    ""word"": ""vicious"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""virus""
    ]
  },
  {
    ""word"": ""victory"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""very""
    ]
  },
  {
    ""word"": ""video"",
    ""rhymes"": [],
    ""soundsLike"": []
  },
  {
    ""word"": ""view"",
    ""rhymes"": [
      ""blue"",
      ""into"",
      ""you"",
      ""review"",
      ""true"",
      ""two"",
      ""shoe"",
      ""crew"",
      ""tissue"",
      ""few"",
      ""glue"",
      ""bamboo"",
      ""undo"",
      ""tattoo"",
      ""renew"",
      ""canoe"",
      ""zoo""
    ],
    ""soundsLike"": [
      ""few""
    ]
  },
  {
    ""word"": ""village"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""voyage"",
      ""valley"",
      ""valid""
    ]
  },
  {
    ""word"": ""vintage"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""vanish"",
      ""village""
    ]
  },
  {
    ""word"": ""violin"",
    ""rhymes"": [
      ""begin"",
      ""spin"",
      ""skin"",
      ""win"",
      ""when"",
      ""twin""
    ],
    ""soundsLike"": []
  },
  {
    ""word"": ""virtual"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""cruel"",
      ""ritual""
    ]
  },
  {
    ""word"": ""virus"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""vicious"",
      ""various""
    ]
  },
  {
    ""word"": ""visa"",
    ""rhymes"": [],
    ""soundsLike"": []
  },
  {
    ""word"": ""visit"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""visa"",
      ""vivid"",
      ""vast""
    ]
  },
  {
    ""word"": ""visual"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""usual"",
      ""casual"",
      ""actual"",
      ""virtual"",
      ""viable""
    ]
  },
  {
    ""word"": ""vital"",
    ""rhymes"": [
      ""title""
    ],
    ""soundsLike"": [
      ""title"",
      ""fatal""
    ]
  },
  {
    ""word"": ""vivid"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""velvet"",
      ""visit"",
      ""divide""
    ]
  },
  {
    ""word"": ""vocal"",
    ""rhymes"": [
      ""local""
    ],
    ""soundsLike"": [
      ""vehicle"",
      ""chuckle"",
      ""vessel"",
      ""local""
    ]
  },
  {
    ""word"": ""voice"",
    ""rhymes"": [
      ""choice""
    ],
    ""soundsLike"": [
      ""choice""
    ]
  },
  {
    ""word"": ""void"",
    ""rhymes"": [
      ""avoid""
    ],
    ""soundsLike"": [
      ""vote"",
      ""avoid""
    ]
  },
  {
    ""word"": ""volcano"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""vacant""
    ]
  },
  {
    ""word"": ""volume"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""vacuum""
    ]
  },
  {
    ""word"": ""vote"",
    ""rhymes"": [
      ""note"",
      ""boat"",
      ""promote"",
      ""float"",
      ""quote"",
      ""goat"",
      ""devote""
    ],
    ""soundsLike"": [
      ""void""
    ]
  },
  {
    ""word"": ""voyage"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""village"",
      ""voice"",
      ""void""
    ]
  },
  {
    ""word"": ""wage"",
    ""rhymes"": [
      ""gauge"",
      ""age"",
      ""engage"",
      ""stage"",
      ""page"",
      ""cage""
    ],
    ""soundsLike"": [
      ""way"",
      ""wish"",
      ""wash"",
      ""page"",
      ""huge""
    ]
  },
  {
    ""word"": ""wagon"",
    ""rhymes"": [
      ""dragon""
    ],
    ""soundsLike"": []
  },
  {
    ""word"": ""wait"",
    ""rhymes"": [
      ""state"",
      ""rate"",
      ""plate"",
      ""great"",
      ""mandate"",
      ""estate"",
      ""gate"",
      ""indicate"",
      ""update"",
      ""debate""
    ],
    ""soundsLike"": [
      ""what"",
      ""wet"",
      ""wheat"",
      ""way"",
      ""waste""
    ]
  },
  {
    ""word"": ""walk"",
    ""rhymes"": [
      ""stock"",
      ""talk"",
      ""lock"",
      ""clock"",
      ""shock"",
      ""hawk"",
      ""flock"",
      ""knock"",
      ""chalk"",
      ""sock"",
      ""unlock""
    ],
    ""soundsLike"": [
      ""hawk"",
      ""work""
    ]
  },
  {
    ""word"": ""wall"",
    ""rhymes"": [
      ""all"",
      ""ball"",
      ""call"",
      ""fall"",
      ""recall"",
      ""small"",
      ""alcohol"",
      ""doll"",
      ""install"",
      ""crawl""
    ],
    ""soundsLike"": [
      ""will"",
      ""wheel"",
      ""whale"",
      ""wool""
    ]
  },
  {
    ""word"": ""walnut"",
    ""rhymes"": [
      ""that"",
      ""nut"",
      ""what"",
      ""robot"",
      ""peanut"",
      ""coconut""
    ],
    ""soundsLike"": [
      ""helmet"",
      ""want"",
      ""unit""
    ]
  },
  {
    ""word"": ""want"",
    ""rhymes"": [
      ""aunt""
    ],
    ""soundsLike"": [
      ""hunt"",
      ""hint""
    ]
  },
  {
    ""word"": ""warfare"",
    ""rhymes"": [
      ""air"",
      ""chair"",
      ""wear"",
      ""square"",
      ""share"",
      ""despair"",
      ""there"",
      ""hair"",
      ""spare"",
      ""aware"",
      ""pair"",
      ""where"",
      ""rare"",
      ""swear"",
      ""pear"",
      ""affair"",
      ""repair"",
      ""prepare"",
      ""glare"",
      ""scare"",
      ""unfair"",
      ""unaware""
    ],
    ""soundsLike"": [
      ""warrior""
    ]
  },
  {
    ""word"": ""warm"",
    ""rhymes"": [
      ""inform"",
      ""uniform"",
      ""swarm"",
      ""reform""
    ],
    ""soundsLike"": [
      ""swarm"",
      ""horn"",
      ""wear""
    ]
  },
  {
    ""word"": ""warrior"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""warfare"",
      ""horror""
    ]
  },
  {
    ""word"": ""wash"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""wish"",
      ""wage"",
      ""wall""
    ]
  },
  {
    ""word"": ""wasp"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""whisper"",
      ""whip""
    ]
  },
  {
    ""word"": ""waste"",
    ""rhymes"": [
      ""taste""
    ],
    ""soundsLike"": [
      ""west"",
      ""wait"",
      ""taste"",
      ""host"",
      ""twist""
    ]
  },
  {
    ""word"": ""water"",
    ""rhymes"": [
      ""daughter""
    ],
    ""soundsLike"": []
  },
  {
    ""word"": ""wave"",
    ""rhymes"": [
      ""brave"",
      ""save"",
      ""cave"",
      ""behave"",
      ""pave""
    ],
    ""soundsLike"": [
      ""way"",
      ""save"",
      ""pave"",
      ""have"",
      ""wage""
    ]
  },
  {
    ""word"": ""way"",
    ""rhymes"": [
      ""day"",
      ""away"",
      ""play"",
      ""say"",
      ""survey"",
      ""display"",
      ""stay"",
      ""delay"",
      ""holiday"",
      ""essay"",
      ""clay"",
      ""betray"",
      ""spray"",
      ""they"",
      ""okay"",
      ""tray"",
      ""obey"",
      ""today""
    ],
    ""soundsLike"": []
  },
  {
    ""word"": ""wealth"",
    ""rhymes"": [
      ""health""
    ],
    ""soundsLike"": [
      ""health"",
      ""will"",
      ""wall""
    ]
  },
  {
    ""word"": ""weapon"",
    ""rhymes"": [],
    ""soundsLike"": []
  },
  {
    ""word"": ""wear"",
    ""rhymes"": [
      ""air"",
      ""chair"",
      ""square"",
      ""share"",
      ""despair"",
      ""there"",
      ""hair"",
      ""spare"",
      ""aware"",
      ""pair"",
      ""where"",
      ""rare"",
      ""swear"",
      ""pear"",
      ""affair"",
      ""repair"",
      ""prepare"",
      ""glare"",
      ""scare"",
      ""warfare"",
      ""unfair"",
      ""unaware""
    ],
    ""soundsLike"": [
      ""where"",
      ""wire"",
      ""hair"",
      ""year"",
      ""weather"",
      ""aware"",
      ""swear""
    ]
  },
  {
    ""word"": ""weasel"",
    ""rhymes"": [
      ""diesel""
    ],
    ""soundsLike"": [
      ""puzzle"",
      ""diesel""
    ]
  },
  {
    ""word"": ""weather"",
    ""rhymes"": [
      ""together""
    ],
    ""soundsLike"": [
      ""where"",
      ""wear"",
      ""winner"",
      ""mother""
    ]
  },
  {
    ""word"": ""web"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""whip"",
      ""hub""
    ]
  },
  {
    ""word"": ""wedding"",
    ""rhymes"": [],
    ""soundsLike"": []
  },
  {
    ""word"": ""weekend"",
    ""rhymes"": [
      ""end"",
      ""friend"",
      ""lend"",
      ""attend"",
      ""trend"",
      ""extend"",
      ""spend"",
      ""depend""
    ],
    ""soundsLike"": [
      ""second"",
      ""wagon""
    ]
  },
  {
    ""word"": ""weird"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""word"",
      ""yard"",
      ""where"",
      ""wear""
    ]
  },
  {
    ""word"": ""welcome"",
    ""rhymes"": [],
    ""soundsLike"": []
  },
  {
    ""word"": ""west"",
    ""rhymes"": [
      ""test"",
      ""best"",
      ""suggest"",
      ""nest"",
      ""chest"",
      ""arrest"",
      ""invest""
    ],
    ""soundsLike"": [
      ""waste"",
      ""wet"",
      ""chest"",
      ""test"",
      ""nest"",
      ""arrest"",
      ""what"",
      ""twist""
    ]
  },
  {
    ""word"": ""wet"",
    ""rhymes"": [
      ""net"",
      ""upset"",
      ""asset"",
      ""forget"",
      ""regret"",
      ""pet"",
      ""sunset""
    ],
    ""soundsLike"": [
      ""what"",
      ""wait"",
      ""wheat""
    ]
  },
  {
    ""word"": ""whale"",
    ""rhymes"": [
      ""scale"",
      ""rail"",
      ""mail"",
      ""detail"",
      ""tail"",
      ""sail"",
      ""female"",
      ""unveil"",
      ""inhale""
    ],
    ""soundsLike"": [
      ""will"",
      ""wall"",
      ""wheel"",
      ""wool"",
      ""way"",
      ""mail""
    ]
  },
  {
    ""word"": ""what"",
    ""rhymes"": [
      ""that"",
      ""nut"",
      ""robot"",
      ""walnut"",
      ""peanut"",
      ""coconut""
    ],
    ""soundsLike"": [
      ""wet"",
      ""wait"",
      ""wheat""
    ]
  },
  {
    ""word"": ""wheat"",
    ""rhymes"": [
      ""seat"",
      ""treat"",
      ""street"",
      ""elite"",
      ""sweet"",
      ""meat"",
      ""retreat"",
      ""athlete"",
      ""repeat""
    ],
    ""soundsLike"": [
      ""wait"",
      ""what"",
      ""wet"",
      ""sweet""
    ]
  },
  {
    ""word"": ""wheel"",
    ""rhymes"": [
      ""deal"",
      ""feel"",
      ""steel"",
      ""real"",
      ""reveal""
    ],
    ""soundsLike"": [
      ""will"",
      ""wall"",
      ""whale"",
      ""wool""
    ]
  },
  {
    ""word"": ""when"",
    ""rhymes"": [
      ""begin"",
      ""spin"",
      ""pen"",
      ""then"",
      ""skin"",
      ""win"",
      ""again"",
      ""hen"",
      ""twin"",
      ""ten"",
      ""violin""
    ],
    ""soundsLike"": [
      ""one"",
      ""win"",
      ""wine"",
      ""wing"",
      ""hen""
    ]
  },
  {
    ""word"": ""where"",
    ""rhymes"": [
      ""air"",
      ""chair"",
      ""wear"",
      ""square"",
      ""share"",
      ""despair"",
      ""there"",
      ""hair"",
      ""spare"",
      ""aware"",
      ""pair"",
      ""rare"",
      ""swear"",
      ""pear"",
      ""affair"",
      ""repair"",
      ""prepare"",
      ""glare"",
      ""scare"",
      ""warfare"",
      ""unfair"",
      ""unaware""
    ],
    ""soundsLike"": [
      ""wear"",
      ""wire"",
      ""hair"",
      ""year"",
      ""weather"",
      ""aware"",
      ""swear""
    ]
  },
  {
    ""word"": ""whip"",
    ""rhymes"": [
      ""ship"",
      ""trip"",
      ""tip"",
      ""clip"",
      ""flip"",
      ""drip"",
      ""hip"",
      ""equip""
    ],
    ""soundsLike"": [
      ""web"",
      ""hip"",
      ""ship"",
      ""tip""
    ]
  },
  {
    ""word"": ""whisper"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""wasp"",
      ""whip""
    ]
  },
  {
    ""word"": ""wide"",
    ""rhymes"": [
      ""side"",
      ""provide"",
      ""ride"",
      ""slide"",
      ""pride"",
      ""tide"",
      ""guide"",
      ""decide"",
      ""divide"",
      ""glide"",
      ""inside"",
      ""outside""
    ],
    ""soundsLike"": [
      ""wood"",
      ""wait"",
      ""what"",
      ""wet"",
      ""wheat""
    ]
  },
  {
    ""word"": ""width"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""wood"",
      ""wide""
    ]
  },
  {
    ""word"": ""wife"",
    ""rhymes"": [
      ""life"",
      ""knife""
    ],
    ""soundsLike"": [
      ""life"",
      ""knife"",
      ""half""
    ]
  },
  {
    ""word"": ""wild"",
    ""rhymes"": [
      ""child""
    ],
    ""soundsLike"": [
      ""world"",
      ""wide"",
      ""child"",
      ""hold""
    ]
  },
  {
    ""word"": ""will"",
    ""rhymes"": [
      ""still"",
      ""drill"",
      ""hill"",
      ""skill"",
      ""pill"",
      ""ill"",
      ""until"",
      ""skull""
    ],
    ""soundsLike"": [
      ""wall"",
      ""wheel"",
      ""whale"",
      ""wool"",
      ""hill"",
      ""wing""
    ]
  },
  {
    ""word"": ""win"",
    ""rhymes"": [
      ""begin"",
      ""spin"",
      ""skin"",
      ""when"",
      ""twin"",
      ""violin""
    ],
    ""soundsLike"": [
      ""when"",
      ""one"",
      ""wine"",
      ""wing"",
      ""twin""
    ]
  },
  {
    ""word"": ""window"",
    ""rhymes"": [],
    ""soundsLike"": []
  },
  {
    ""word"": ""wine"",
    ""rhymes"": [
      ""design"",
      ""sign"",
      ""fine"",
      ""genuine"",
      ""shine"",
      ""decline"",
      ""define"",
      ""online"",
      ""combine""
    ],
    ""soundsLike"": [
      ""one"",
      ""win"",
      ""when""
    ]
  },
  {
    ""word"": ""wing"",
    ""rhymes"": [
      ""ring"",
      ""bring"",
      ""spring"",
      ""swing"",
      ""sting"",
      ""thing"",
      ""sing""
    ],
    ""soundsLike"": [
      ""win"",
      ""when"",
      ""one"",
      ""will"",
      ""wine"",
      ""swing"",
      ""wink""
    ]
  },
  {
    ""word"": ""wink"",
    ""rhymes"": [
      ""drink"",
      ""link"",
      ""pink""
    ],
    ""soundsLike"": [
      ""wing"",
      ""pink"",
      ""link""
    ]
  },
  {
    ""word"": ""winner"",
    ""rhymes"": [
      ""dinner"",
      ""inner""
    ],
    ""soundsLike"": [
      ""win"",
      ""winter"",
      ""when"",
      ""one"",
      ""weather"",
      ""wear""
    ]
  },
  {
    ""word"": ""winter"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""wonder"",
      ""winner"",
      ""want"",
      ""water""
    ]
  },
  {
    ""word"": ""wire"",
    ""rhymes"": [
      ""fire"",
      ""inspire"",
      ""acquire"",
      ""require"",
      ""hire"",
      ""entire"",
      ""liar"",
      ""retire"",
      ""expire"",
      ""buyer""
    ],
    ""soundsLike"": [
      ""where"",
      ""wear"",
      ""hire"",
      ""wise""
    ]
  },
  {
    ""word"": ""wisdom"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""wedding"",
      ""system""
    ]
  },
  {
    ""word"": ""wise"",
    ""rhymes"": [
      ""exercise"",
      ""demise"",
      ""size"",
      ""surprise"",
      ""prize""
    ],
    ""soundsLike"": [
      ""wire""
    ]
  },
  {
    ""word"": ""wish"",
    ""rhymes"": [
      ""fish"",
      ""dish""
    ],
    ""soundsLike"": [
      ""wash"",
      ""will"",
      ""wage""
    ]
  },
  {
    ""word"": ""witness"",
    ""rhymes"": [
      ""fitness""
    ],
    ""soundsLike"": [
      ""fitness""
    ]
  },
  {
    ""word"": ""wolf"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""wool"",
      ""will"",
      ""wall"",
      ""wife"",
      ""wealth"",
      ""wheel"",
      ""whale""
    ]
  },
  {
    ""word"": ""woman"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""human""
    ]
  },
  {
    ""word"": ""wonder"",
    ""rhymes"": [
      ""under"",
      ""thunder""
    ],
    ""soundsLike"": [
      ""winter"",
      ""winner"",
      ""render"",
      ""vendor""
    ]
  },
  {
    ""word"": ""wood"",
    ""rhymes"": [
      ""good"",
      ""hood""
    ],
    ""soundsLike"": [
      ""wide"",
      ""word"",
      ""hood"",
      ""wait"",
      ""what"",
      ""wet"",
      ""wheat""
    ]
  },
  {
    ""word"": ""wool"",
    ""rhymes"": [
      ""pull""
    ],
    ""soundsLike"": [
      ""will"",
      ""wall"",
      ""wheel"",
      ""whale"",
      ""wolf"",
      ""pull""
    ]
  },
  {
    ""word"": ""word"",
    ""rhymes"": [
      ""bird"",
      ""absurd""
    ],
    ""soundsLike"": [
      ""wood"",
      ""world"",
      ""worry"",
      ""wide"",
      ""weird""
    ]
  },
  {
    ""word"": ""work"",
    ""rhymes"": [
      ""network"",
      ""clerk"",
      ""artwork""
    ],
    ""soundsLike"": [
      ""walk"",
      ""worth"",
      ""worry""
    ]
  },
  {
    ""word"": ""world"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""word"",
      ""wild""
    ]
  },
  {
    ""word"": ""worry"",
    ""rhymes"": [
      ""hurry""
    ],
    ""soundsLike"": [
      ""hurry"",
      ""work"",
      ""word"",
      ""worth""
    ]
  },
  {
    ""word"": ""worth"",
    ""rhymes"": [
      ""earth"",
      ""birth""
    ],
    ""soundsLike"": [
      ""work"",
      ""worry"",
      ""word"",
      ""birth""
    ]
  },
  {
    ""word"": ""wrap"",
    ""rhymes"": [
      ""snap"",
      ""trap"",
      ""gap"",
      ""scrap"",
      ""clap""
    ],
    ""soundsLike"": [
      ""trap"",
      ""rib"",
      ""ramp""
    ]
  },
  {
    ""word"": ""wreck"",
    ""rhymes"": [
      ""check"",
      ""neck""
    ],
    ""soundsLike"": [
      ""rack"",
      ""rug""
    ]
  },
  {
    ""word"": ""wrestle"",
    ""rhymes"": [
      ""vessel""
    ],
    ""soundsLike"": []
  },
  {
    ""word"": ""wrist"",
    ""rhymes"": [
      ""list"",
      ""just"",
      ""twist"",
      ""assist"",
      ""exist"",
      ""resist"",
      ""enlist""
    ],
    ""soundsLike"": [
      ""arrest"",
      ""roast""
    ]
  },
  {
    ""word"": ""write"",
    ""rhymes"": [
      ""light"",
      ""right"",
      ""night"",
      ""sight"",
      ""flight"",
      ""bright"",
      ""slight"",
      ""height"",
      ""kite"",
      ""invite"",
      ""excite"",
      ""midnight""
    ],
    ""soundsLike"": [
      ""right"",
      ""ride"",
      ""rate"",
      ""route"",
      ""bright""
    ]
  },
  {
    ""word"": ""wrong"",
    ""rhymes"": [
      ""long"",
      ""strong"",
      ""song""
    ],
    ""soundsLike"": [
      ""ring"",
      ""around"",
      ""rain""
    ]
  },
  {
    ""word"": ""yard"",
    ""rhymes"": [
      ""card"",
      ""guard"",
      ""hard""
    ],
    ""soundsLike"": [
      ""hard"",
      ""card"",
      ""weird"",
      ""guard""
    ]
  },
  {
    ""word"": ""year"",
    ""rhymes"": [
      ""deer"",
      ""pioneer"",
      ""appear"",
      ""near"",
      ""sphere""
    ],
    ""soundsLike"": [
      ""where"",
      ""wear""
    ]
  },
  {
    ""word"": ""yellow"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""hello""
    ]
  },
  {
    ""word"": ""you"",
    ""rhymes"": [
      ""blue"",
      ""into"",
      ""view"",
      ""review"",
      ""true"",
      ""two"",
      ""shoe"",
      ""crew"",
      ""tissue"",
      ""few"",
      ""glue"",
      ""bamboo"",
      ""undo"",
      ""tattoo"",
      ""renew"",
      ""canoe"",
      ""zoo""
    ],
    ""soundsLike"": []
  },
  {
    ""word"": ""young"",
    ""rhymes"": [
      ""tongue"",
      ""among""
    ],
    ""soundsLike"": [
      ""wing"",
      ""one"",
      ""tongue""
    ]
  },
  {
    ""word"": ""youth"",
    ""rhymes"": [
      ""truth"",
      ""tooth""
    ],
    ""soundsLike"": [
      ""you"",
      ""use"",
      ""tooth""
    ]
  },
  {
    ""word"": ""zebra"",
    ""rhymes"": [],
    ""soundsLike"": [
      ""zero""
    ]
  },
  {
    ""word"": ""zero"",
    ""rhymes"": [
      ""hero""
    ],
    ""soundsLike"": [
      ""hero""
    ]
  },
  {
    ""word"": ""zone"",
    ""rhymes"": [
      ""bone"",
      ""stone"",
      ""tone"",
      ""loan"",
      ""phone"",
      ""own"",
      ""alone"",
      ""ozone"",
      ""unknown""
    ],
    ""soundsLike"": [
      ""ozone""
    ]
  },
  {
    ""word"": ""zoo"",
    ""rhymes"": [
      ""blue"",
      ""into"",
      ""you"",
      ""view"",
      ""review"",
      ""true"",
      ""two"",
      ""shoe"",
      ""crew"",
      ""tissue"",
      ""few"",
      ""glue"",
      ""bamboo"",
      ""undo"",
      ""tattoo"",
      ""renew"",
      ""canoe""
    ],
    ""soundsLike"": []
  }
]
";

    }
}
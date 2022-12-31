using System;
using System.IO;
using Newtonsoft.Json;

namespace FixMyCrypto {
    class Checkpoint {
        private Phrase phrase = null;

        private string passphrase = null;

        private long passphraseNum = -1;
        private long passphraseTotal = 0;
        private long phraseTotal = 0;
        private int passphraseMaxLength = 0;

        private PhraseProducer pp;

        private PhraseToAddress p2a;

        private System.Timers.Timer timer;

        private object mutex;

        public Checkpoint(int interval = 30) {
            mutex = new object();
            timer = new System.Timers.Timer(interval * 1000);
            timer.Elapsed += (StringReader, args) => { 
                SaveCheckpoint();
            };
        }

        public void Start() {
            timer.Start();
        }

        public void Stop() {
            timer.Stop();
        }

        public void SetPhraseProducer(PhraseProducer pp) {
            this.pp = pp;
            this.pp.SetCheckpoint(this);
        }

        public void SetPhraseToAddress(PhraseToAddress p2a) {
            this.p2a = p2a;
            this.p2a.SetCheckpoint(this);
        }

        public Phrase GetCheckpointPhrase() {
            return this.phrase;
        }

        public void ClearPhrase() {
            this.phrase = null;
        }

        public (string, long) GetCheckpointPassphrase() {
            return (this.passphrase, this.passphraseNum);
        }

        public long GetPassphraseTotal() {
            return this.passphraseTotal;
        }

        public long GetPhraseTotal() {
            return this.phraseTotal;
        }

        public void SetPhraseTotal(long phraseTotal) {
            this.phraseTotal = phraseTotal;
        }

        public int GetPassphraseMaxLength() {
            return this.passphraseMaxLength;
        }

        public void ClearPassphrase() {
            this.passphrase = null;
            this.passphraseNum = -1;
        }

        private void SaveCheckpoint() {
            if (pp == null || p2a == null || Global.Done || Global.Found) return;

            try {
                lock(mutex) {
                    (Phrase phrase, string passphrase, long passphraseNum, long passphraseTotal, int maxLength) = p2a.GetLastTested();

                    if (phrase == null) {
                        return;
                    }

                    //  Write to checkpoint file

                    var checkpoint = new {
                        phrase = phrase.ToPhrase(),
                        phraseNum = phrase.SequenceNum,
                        phraseTotal = phraseTotal,
                        passphrase = passphrase,
                        passphraseNum = passphraseNum,
                        passphraseTotal = passphraseTotal,
                        passphraseMaxLength = maxLength
                    };

                    string json = JsonConvert.SerializeObject(checkpoint, Formatting.Indented);
                    StreamWriter writer = File.CreateText("checkpoint.json");
                    writer.WriteLine(json);
                    writer.Close();
                    Log.Debug("Updated checkpoint file");
                }
            }
            catch (Exception e) {
                Log.Error($"Error writing checkpoint file: {e}");
                throw;
            }
        }

        public bool RestoreCheckpoint() {
            try {
                lock(mutex) {
                    //  Read from checkpoint file
                    string str = File.ReadAllText("checkpoint.json");
                    dynamic result = JsonConvert.DeserializeObject(str);
                    this.phrase = new Phrase((string)result.phrase, (long)result.phraseNum.Value);
                    this.phraseTotal = result.phraseTotal.Value;
                    this.passphrase = result.passphrase;
                    this.passphraseNum = result.passphraseNum.Value;
                    this.passphraseTotal = result.passphraseTotal.Value;
                    this.passphraseMaxLength = (int)result.passphraseMaxLength.Value;
                    Log.Info($"Restoring from checkpoint:\n\tphrase: {this.phrase.ToPhrase()}\n\tpassphrase: {this.passphrase}");
                    return true;
                }
            }
            catch (Exception e) {
                Log.Error($"Error reading checkpoint file: {e}");
                ClearPhrase();
                ClearPassphrase();
                throw;
            }
        }
    }
}
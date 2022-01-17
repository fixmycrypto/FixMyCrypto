using System;
using System.IO;
using Newtonsoft.Json;

namespace FixMyCrypto {
    class Checkpoint {
        private Phrase phrase = null;

        private string passphrase = null;

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

        public Phrase? GetCheckpointPhrase() {
            return this.phrase;
        }

        public void ClearPhrase() {
            this.phrase = null;
        }

        public string? GetCheckpointPassphrase() {
            return this.passphrase;
        }

        public void ClearPassphrase() {
            this.passphrase = null;
        }

        private void SaveCheckpoint() {
            if (pp == null || p2a == null || Global.Done || Global.Found) return;

            try {
                lock(mutex) {
                    Phrase phrase = pp.GetLastPhrase();
                    string passphrase = p2a.GetLastPassphrase();

                    //  Write to checkpoint file

                    var checkpoint = new {
                        phrase = phrase.ToPhrase(),
                        passphrase = passphrase
                    };

                    string json = JsonConvert.SerializeObject(checkpoint, Formatting.Indented);
                    StreamWriter writer = File.CreateText("checkpoint.json");
                    writer.WriteLine(json);
                    writer.Close();
                    Log.Info("Wrote checkpoint file");
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
                    this.phrase = new Phrase((string)result.phrase);
                    this.passphrase = result.passphrase;
                    Log.Info($"Restoring from checkpoint\nphrase: {this.phrase.ToPhrase()}\npassphrase: {this.passphrase}");
                    return true;
                }
            }
            catch (Exception e) {
                Log.Error($"Error reading checkpoint file: {e}");
                this.phrase = null;
                this.passphrase = null;
                throw;
            }
            return false;
        }
    }
}
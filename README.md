# Caution!

**Never share your recovery phrase (seed phrase, mnemonic words) with ANYONE!** Anyone asking for your recovery phrase is a scammer. Never type it into any website or any internet-connected computer or device.

# FixMyCrypto Features

* Runs totally offline so your recovery phrase is never exposed to the internet
* Automatically fixes most common mistakes which result in an "invalid recovery phrase" error:
    * Invalid words (e.g. fax -> fix)
    * Incorrect words (e.g. fix -> fox/fit/fog etc.)
    * Swapped word order
    * Missing or unknown words
* BIP39 passphrase cracking, including: typo repair, wildcards, and brute forcing
* Coin support: 
    * BTC (+ forks e.g. BCH, etc.)
    * ETH (+ forks e.g. ETC, BSC, etc.)
    * LTC
    * DOGE
    * ADA (Cardano)
    * SOL
    * ALGO
    * DOT (Polkadot)
    * XRP (Ripple)
    * ATOM (Cosmos)
    * Crypto.org chain (Cronos / CRO)
    * Need another coin? Let us know!
* [GPU acceleration](#GPU-Acceleration)
    * PBKDF2 hashing for all cryptos
    * Secp256k1 ECC full path derivation for BTC & ETH (and their altcoins), XRP, ATOM, CRO
    * Currently working on NVIDIA GPUs only
* Smart typo detection drastically reduces the search time
    * Phrase words are prioritized based on spelling and pronunciation similarity as well as keyboard distance (most likely typos)
* Support for special Ledger/Trezor hardware key derivation modes used for Cardano, Algorand, and Polkadot
* Simultaneous search of multiple derivation paths (including non-standard paths)
* Search a specified range of accounts and indices
* Highly multi-threaded, efficient key reuse when searching multiple paths/accounts/indices
* Checkpoints to resume progress if interrupted

# Support

For technical support or help with any crypto recovery needs, please contact us via [e-mail](mailto:help@fixmycrypto.com) or via our website <https://www.fixmycrypto.com>

# Donations / Tips

Please consider donating to fund future development.

* BTC: bc1q477afku8x7964gmzlsapgj8705e63ch89p8k4z
* ETH: 0x0327DF6652D07eE6cc670626b034edFfceD1B20C
* DOGE: DT8iZF8RbqpRftgrWdiq34EZdJpCGiWBwG
* ADA: addr1qxhjru35kv8fq66afxxdnjzf720anfcppktchh6mjuwxma3e876gh3czzkq0guls5qrkghexsuh543h7k2xqlje5lskqfp2elv
* SOL: 7ky2LTXNwPASogjMURv88LoPRHAAL4v49HeD7MYARuM4
* ALGO: EPQZU6GMEMKKEQH4CP7U2U2NTQE2ZVMOYAS7F5WMCUYIAYUKNJVUHW5W5A
* DOT: 14jUHiE429X8HwPRmj2Sy4Xvo5Z9ewJJJ273ctvmQgxgTJ4b
* XRP: rJtr6VfAP5Qmp2abKUfdJEGtHEckRpcKHk
* ATOM: cosmos159zgwcx454uqrtrva8nk3sdrlegdxxln2w20s3
* CRO: cro1yk3yuul2f60mfqy93fs8dcf3flt40xv9mtqef6

# How to use

1. Download the latest release from the Releases tab (or see [`BUILD.md`](BUILD.md) to build from source)
2. Extract the .zip file to a folder
3. (Optional) Run `FixMyCrypto` to watch it run with the default included settings (using a test recovery phrase)
4. **Disconnect from the network (unplug Ethernet cable, shut off WiFi).**
5. Edit the `settings.json` file, filling in the details as described below. At a **minimum**, you must provide:
    * [Coin](#coin)
    * [Phrase](#phrase)
    * [Passphrase](#passphrase) (if used)
    * [Known address](#known-addresses) (at least one)
6. Run `FixMyCrypto`

# Checkpoints

The current progress will be saved every 30 seconds to the file `checkpoint.json` so you can resume a long running job if it gets interrupted. If this file exists when the program starts, it will resume near where it left off. If you cancel a job and change your settings or need to restart from the beginning, you should delete `checkpoint.json`.

# Paranoid "Amnesia" Environment

If you want to be extra careful, you can run the software in an amnesiac environment which doesn't save any files to your hard drive, such as [`Tails Linux`](https://tails.boum.org). You still need to disconnect from the network before entering your phrase, but this ensures that no traces will be left behind once you reboot your system. Note that in the case of a power failure or reboot, your progress will be lost since the checkpoint file won't persist after a reboot.

# GPU Acceleration

GPU acceleration greatly increases the speed of the search, especially if you have a CPU with a lower core count, e.g. a quad core CPU paired with a fast gaming GPU. This currently works on NVIDIA GPUs only. 

To verify that OpenCL is installed (it should be automatically instaleld by the NVIDIA drivers), you can run this command to see a list of available GPUs and benchmark them:

    FixMyCrypto -opencl

You should see a table that looks like this:

| Platform       | Device                        | Global Memory | Local Memory | CUs |
| --- | --- | --- | --- | --- |
| 0: NVIDIA CUDA | 0: NVIDIA GeForce RTX 3060 Ti | 8 GiB         | 48 KiB       | 38  |

If you have a single GPU, it will probably be Platform 0, Device 0 (unless you have other OpenCL drivers installed such as the Intel ICD). If you have multiple devices, you should choose the best platform and then 1 or more devices from that platform. (You cannot simultaneously use more than 1 platform.)

To run the search in GPU mode, you can either set the platform & device in your `settings.json` file:

    "platform": 0,
    "device": 0

Alternatively, you can specify the platform & device on the command line:

    FixMyCrypto -platform 0 -device 0

Multiple devices:

    "platform": 0,
    "devices": [ 0, 1, 2 ]

Or:

    FixMyCrypto -platform 0 -devices 0,1,2

---

# Configuration (settings.json file)

## Coin

### Required

Specify which cryptocurrency you are searching for. (`BTC`, `ETH`, `ADA`, `DOGE`, `LTC`, `SOL`, `ALGO`, `DOT`, `XRP`, `ATOM`, `CRO`, etc.). (For ADA or DOT used with a Ledger or Trezor hardware wallet, see the relevant [special use cases](#special-use-cases) section.)

    "coin": "BTC",

If you have many coins tied to the same phrase, pick any coin for which you are sure you know the first address (account 0, index 0) which will speed up the search.

## Phrase

### Required

Enter your recovery phrase ("seed phrase" or "mnemonic phrase") between the quotation marks. The total number of words must be the same number as the length of your original recovery phrase (12, 15, 18, or 24 words for most cryptos; 25 words for `My Algo` wallets). 

    "phrase": "apple banana pear watermelon kiwi strawberry apple banana pear watermelon kiwi strawberry",

### Locked Words

If you are certain that some of the phrase words are correct (both the position and the spelling), but not others, you can put a exclamation point (`!`) after the word(s) you're certain are correct. These locked words will not be substituted or swapped, which will speed up the search.

E.g. if you're 100% sure that the first six words are correct, but not certain about the latter six words:

    "phrase": "apple! banana! pear! watermelon! kiwi! strawberry! apple banana pear watermelon kiwi strawberry",

Note: If you lock a word that turns out to be incorrect, the recovery will fail. Only do this for words you're 100% certain are correct!

### Unknown / Invalid Words

If you know the position of a word but you have no idea what that word is supposed to be, replace it with an asterisk (`*`). For example, if you're certain that you're missing the 2nd word:

    "phrase": "apple * pear watermelon kiwi strawberry apple banana pear watermelon kiwi pear",

**Hint:** You might be tempted to replace any invalid words (words you wrote down that aren't on the BIP39 list) with a valid word from the list or a `*`, but it's actually better to leave any mistakes as-is. By leaving the invalid words in place, the software will immediately know which word(s) need to be changed first, instead of needing to check every word in the phrase. Also, the program will likely do a better job than you of guessing which typos were made and which replacement words should be tested.

Repairing up to 3 invalid / incorrect words is typically feasible, sometimes 4 if the typos aren't too bad, but each additional incorrect word will increase the search time exponentially.

### Missing Words

If you are missing some words and don't know where they go or which position(s) are missing, add a question mark (`?`) to the end of the phrase for each missing word. For example, if you only have 11 out of 12 words:

    "phrase": "apple banana pear watermelon kiwi strawberry apple banana pear watermelon kiwi ?",

A single missing word (one `?`) can be solved quickly, two missing words will take a few hours, but 3 or more missing words can take a VERY long time to solve, since the program must try every possible word in every possible position.

### Multiple Phrases

If you have multiple unrelated recovery phrases and you're not sure which one is associated with your address, you can specify multiple phrases as an array like this:

    "phrase": [
        "first phrase to test",
        "second phrase to test",
        "third phrase to test"
    ],

Only do this if you're really not sure which phrase is associated with your address, and try to put the most likely phrase first and the least likely phrase last. All likely permutations of the first phrase will be tested first.

---

## Passphrase

### Required

A BIP39 passphrase (a.k.a. "extra word", "hidden wallet", or "advanced security") is not to be confused with your spending password, wallet password, or app login password.

If you didn't use a passphrase when you created the wallet or you're not sure what this is, then leave this as blank:

    "passphrase": "",

If you used a passphrase and you are 100% certain of the exact passphrase you used, specify it here:

    "passphrase": "ThePassphrase!",

If you used a passphrase and you're 100% certain it's one from a list of possible passphrases, you can specify an array of passphrases to test:

    "passphrase": [

        "FirstPossibility",

        "SecondPossibility"
    ],

This will match a passphrase of "FirstPossibility" or "SecondPossibility" **(exact matches only)**.

Hint: A fairly common mistake is setting your PIN, spending password, or other passwords as your passphrase. If there's any chance you may have done this, try setting these for the passphrase in case that's what happened. Also add a blank `""` as one of the options too, in case you didn't use any passphrase.

### Example

You're not sure if you used any passphrase, but your PIN was `8675309` and your spending password was `SpendingPassword`.

    "passphrase": [

        "",

        "8675309",

        "SpendingPassword"
    ]

If you think you may have done this AND also possibly made a typo when setting the passphrase, continue reading the next section.

## Passphrase Fuzzing

Fuzzing is used when you may have made typos in your passphrase when you created the wallet or wrote down the passphrase. Fuzzing will test all possible typo mistakes, including: insertions, deletions, substitutions, transpositions, and all upper/lower case (e.g. caps lock left on).

To use passphrase fuzzing, surround the passphrase (or a part of it) with curly braces `{` and `}`:

    "passphrase": "{ThePassphrase!}",

This will test e.g. "ThePass**hp**rase!" and all other single typos of "ThePassphrase!" (2,848 permutations).

### Fuzz Multiple Passphrases

You can fuzz multiple passphrases if you're not sure which one you used:

    "passphrase": [

        "{FirstPossibility}",

        "{SecondPossibility}
    ],

This will perform a fuzzy match of all possible typos of "FirstPossibility" and "SecondPossibility" (6,720 permutations).

### Fuzz Partial Passphrase

You can fuzz just one part of the passphrase, if you're **certain** that the typo is in that part and nowhere else:

    "passphrase": "Perfect{Mistake}",

### Fuzz Multiple Typos

If you think you have more than 1 typo in your passphrase, you can put double curly braces to test for 2 typos (this increases the search time exponentially):

    "passphrase": "{{ThePassphrase!}}",

This will test e.g. "ThePass**hp**rase**1**" and all other single **or double** typos of "ThePassphrase!" (8,387,574 permutations).

**Trying to brute force more than 2 typos in a passphrase will fail due to the huge number of permutations**.  If you have at least some idea as to where the typo(s) were made, then try using wildcards.

## Passphrase Wildcards

If you have a good idea of the components that make up the passphrase, but not the exact order or exact characters, you can use the following wildcards. Each wildcard used will increase the search time exponentially. **Brute forcing the entirety of a long passphrase is not feasible.**

* `( )` parenthesis contain Boolean expressions using `&&` (and) and `||` (or) operators
    * `(Correct||Horse||Battery)` will match "Correct", "Horse", OR "Battery"
    * `(Correct&&Horse&&Battery)` will match "CorrectHorseBattery", "CorrectBatteryHorse", "HorseCorrectBattery", etc. (all permutations of Correct AND Horse AND Battery)
    * You can nest Boolean expressions:
        * `((a||b)&&c)` will match "ac", "bc", "ca", or "cb" (but NOT "ab" or "abc")
        * `((C||c)orrect&&(H||h)orse)` will match "CorrectHorse", "Correcthorse", "HorseCorrect", "horsecorrect", etc.
    * However, you cannot mix and/or operators in a single Boolean expression:
        * `(a&&b||c)` is not valid
* `[ ]` square bracket expressions match one of any of the characters or ranges contained within:
    * `[a-z]` will match one lower case letter: a-z
    * `[a-zA-Z]` will match one lower OR upper case letter: a-z OR A-Z
    * `[aeiou]` will match one lower case vowel letter
    * `[0-9]` will match one digit
    * `[!@#$%^&*()]` will match one of the listed special symbols
    * `([)`, `(])` use parenthesis to escape a square bracket in the passphrase
    * `[(]`, `[)]` use square brackets to escape a parenthesis in the passphrase
    * `[{]`, `[}]` use square brackets around a curly brace to escape it
    * `[?]` will escape a question mark (only needed if it comes immediately after a right square bracket, right parenthesis, or right curly brace)
    * `^` at the start of a square bracket expression means to exclude all the listed items, i.e. match any ASCII printable character except for those that are listed.
        * `[^a-zA-Z]` will match any non-letter character (matches one digit or symbol)
        * `[^a-zA-Z0-9]` will match any non-alphanumeric character
        * `[^^]` Two carets will escape a caret (matches "^"); `[^^$]` will match "^" or "$"
    * Nested square bracket expressions are not allowed
* `{ }` curly brace expressions will fuzz the contents by testing all possible single tyops (insertions, deletions, substitutions, and transpositions)
    * `{{ }}` double curly braces will test all possible double typos
* `?` after an enclosed expression indicates that the enclosed item is optional (i.e. it occurs zero or one times)
    * `(T|t)?he` will match "The", "the", or "he"
    * `Hello Dolly[!$]?` will match "Hello Dolly", "Hello Dolly!", or "Hello Dolly$"
    * `[0-9][0-9]?` will match any one or two digits 0 - 9 and 00 - 99 (including 00, 01, etc.)
    * `[1-9]?[0-9]` will match any one or two digit number 0 - 99 (but NOT 00, 01, etc.)

### Example 1

Your passphrase contains the words "hello" and "dolly" (in that order, with uncertain capitalization), followed by one symbol out of `!@#$%^&*`, and finally one or two digits:

    "passphrase": "(H||h)ello(D||d)olly[!@#$%^&*][0-9][0-9]?",

This would match: 
* "hellodolly!1"
* "HelloDolly$42"
* "Hellodolly*69", etc. (3,520 permutations)

### Example 2

Your passphrase contains the words "correct", "horse", "battery", and "staple", in unknown order or capitalization, followed by a one or two digit number, and then one non-alphanumeric symbol:

    "passphrase": "((C||c)orrect&&(H||h)orse&&(B||b)attery&&(S||s)taple)[1-9]?[0-9][^a-zA-Z0-9]",

This would match: 
* "CorrectHorseBatteryStaple1!"
* "horseStaplebatteryCorrect42?"
* "batterystaplecorrectHorse99@", etc. (1,267,200 permutations)

### Example 3

You're certain that your passphrase is supposed to be "CorrectHorseBatteryStaple42!", but it doesn't generate matching addresses in your wallet. Try passphrase fuzzing:

    "passphrase": "{CorrectHorseBatteryStaple42!}",

This would match all possible single typos:
* "CorrectHorseBatterySta**b**le42!"
* "Co**r**ectHorseBatteryStaple42!"
* "CorrectHo**o**rseBatteryStaple42!"
* "correcthorsebatterystaple42!", "CORRECTHORSEBATTERYSTAPLE42!" etc. (5,795 permutations)

### Example 4

You're certain your passphrase was either "MyUsualPassword", "MyOtherPassword", or "4321", but none of those works. You can try fuzzing all of them simultaneously:

    "passphrase": [

        "{MyUsualPassword}",

        "{MyOtherPassword},

        "{4321}"
    ],

This will match typos of any of those passphrases, e.g.:
* "MyUsualPassw**a**rd"
* "MyOt**t**erPassword"
* "4**23**1", etc. (6965 permutations)

---

## Known Addresses

### Required

Provide 1 or more addresses that you are certain belong to this wallet. Ideally, you should put the first address that was created by the wallet (account 0, index 0). If you aren't sure which address is address 0, provide as many addresses as you can. Adding more addresses here doesn't slow the search down, in fact it is more likely to speed things up. Check your transaction history with exchanges, wallets, e-mail receipts, etc. to find the receive address(es) you used with this wallet. 

In the case of Ethereum, only one address (index 0) is typically used because the same address is used for every transaction on the same account.

### Example for one known address

    "knownAddresses":  [
        "bc1qmr9cmy3p3q695mkn5rnmz27csvnvr2ja05wa4p"
    ],

If the address you provide is not the first address for your first account (account 0, index 0), then you probably need to set the indices and accounts ranges as described below.

### Example for multiple known addresses

    "knownAddresses":  [
            "bc1qmr9cmy3p3q695mkn5rnmz27csvnvr2ja05wa4p",

            "1BzeytZbmdDt4678V4MnCEp2m2bM42Ey4p",

            "3A7Wg827LsxcEiaMmpeaKoKMGPfUfCGH4X",
        ],

---

## Indices

### Recommended

"indices" specifies the range of address indices to check against. UTXO-based blockchains such as Bitcoin and Cardano use a different address index (new address) for each transaction. Each index represents one address from the wallet. 

The default setting is to check if the known address(es) are in the first 5 addresses of the wallet, hence `"0-4"`. This means one of the known addresses you provide must belong to one of the first five transactions received by this wallet. If you know that your address is between index 5 and 10, use `"5-10"`. Ranges can be specified using hyphens: e.g. `"0-5"`, commas: e.g. `"2,4,6"`, or a mix of both e.g.: `"0,2,4,10-12"`. The more indices you search, the longer it will take. Ideally, if you know you have provided your address 0, then you can set this to `"0"` to speed things up a bit. However, if your known address is address index 5, and you specify the range as `"0-4"`, it won't find your address at all. **Make sure this range is big enough to include at least one of the known addresses you've provided.**

ETH and SOL typically only use address index 0, so you should change this to `"0"` to speed things up. Your other ETH or SOL addresses will typically belong to a different account number instead of a different index, see below.

ALGO uses neither index nor account numbers.

## Accounts

### Recommended

Similar to indices, "accounts" specifies the range of accounts to check against. For most users, if you had only one account for a particular coin tied to this recovery phrase, the account should be set to `"0"`. As with "indices", you can specify a range using hyphens and commas.

## Paths

### Optional (usually)

This specifies the derivation path(s) used to generate addresses. Most users can leave this blank to search using default derivation paths. In most cases we can detect the path from the address format or the default paths commonly used by the coin. If you know for certain which path your wallet used, you can specify it here to speed up the search. If you used a particularly old wallet software or one known to use a non-standard derivation path, it may be necessary to specify a custom path here. You can specify one or more paths. The more paths you specify, the longer the search will take. Use `{account}` as the placeholder for the account number, and `{index}` as the placeholder for the address index.

If you happen to know for sure which path your wallet used, you can speed up the search by specifying it:

### Example

    "paths": [
        "m/44'/0'/{account}'/{index}"
    ],

Note that the search will fail if none of the paths you specify match the one used to generate your wallet addresses.

### Bitcoin

By default, the program searches these derivation paths for BTC addresses:

Addresses that start with 1... (Legacy):

* `m/44'/0'/{account}'/0/{index}` - Modern BIP44 compatible wallets
* `m/44'/0'/{account}'/{index}` - Coinomi, blockchain.com, old Ledger versions
* `m/0'/0'/{index}'` - Bitcoin Core
* `m/0'/0/{index}` - Multibit HD, BRD wallet

Addresses that start with 3... (Segwit):

* `m/49'/0'/{account}'/0/{index}` - BIP49

Addresses that start with bc1q (Native Segwit):

* `m/84'/0'/{account}'/0/{index}` - BIP84

Addresses that start with bc1p or tb1 (Taproot):

* `m/86'/0'/{account}'/0/{index}` - BIP86

### Ethereum

By default, the program searches two derivation paths for ETH addresses:

* `m/44'/60'/{account}'/0/{index}` - Used by most up-to-date wallets
* `m/44'/60'/{account}'/{index}` - Used by Coinomi and old Ledger/MEW versions (2019 or older)

If you used older versions of Metamask together with a Ledger hardware wallet, some users noted that it used an incorrect account value (e.g. 10) instead of 0 to generate addresses. In this case you can specify the account range to search as `"0,10"` (only accounts 0 and 10), or `"0-10"` (accounts 0 through 10). It's possible that other account numbers may have been used as well, so you may need to specify a larger range. Note that searching more account numbers increases the search time.

    "accounts": "0-10",

See https://medium.com/myetherwallet/hd-wallets-and-derivation-paths-explained-865a643c7bf2 for other possible paths used by ETC and other ETH-derived coins. You can search multiple paths at the same time if you aren't sure which one to use.

---
## Special use cases

### Ethereum Classic (ETC)

Use "ETH" for the coin type, and use one or more of the following paths:

* `m/44'/60'/{account}'/0/{index}` – Pre-split ETH/ETC wallet
* `m/44'/61'/{account}'/0/{index}` – Post-split ETC wallet
* `m/44'/60'/160720'/0/{index}` – Ledger ETC
* `m/44'/60'/160720'/0'/{index}` – Old Ledger, Vintage MEW

### Cardano (ADA)

Ledger & Trezor hardware wallets generate your Cardano private key differently from the official spec used by software wallets like Daedalus, Yoroi, and Adalite. This means the same recovery phrase will create a different wallet (different addresses) depending on whether you use the phrase on a Ledger, Trezor, or software wallet. If you used a Ledger or Trezor wallet for Cardano and need to recover it, use the coin type `ADATrezor` or `ADALedger` to recover your address, otherwise the addresses won't match and recovery won't be possible.

    "coin": "ADALedger",

### Algorand (ALGO)

`My Algo` software wallet for Algorand uses a 25 word phrase (not including a passphrase) and the derivation path `m`.

Ledger hardware wallets use a 12-24 word phrase (plus optional passphrase), and the derivation path `m/44'/283'/{account}'/0/{index}`.

If you happen to know which one you need, specifying the path will speed things up a bit:

    "paths": [
        "m/44'/283'/{account}'/0/{index}"
    ],

Other wallet software may use different phrases or paths.

### Polkadot (DOT)

For the coin `DOT`, Polkadot{.js} Extension style addresses (using sr25519 signatures) are supported. This includes Polkadot, Kusama, and Generic Substrate addresses.

Recovering a Ledger wallet for DOT requires specifying the coin: `DOTLedger`:

    "coin": "DOTLedger",

DOTLedger addresses should start with a `1`.

Other address formats (e.g. secp256k signatures) or custom derivation paths are not currently supported. For more info, see: <https://wiki.polkadot.network/docs/learn-accounts#portability>

---

## Other settings

* `difficulty` (default 0, range 0-4) controls which algorithms (different types of word substitutions, swaps, etc.) will be tested. Leave this at default unless the program fails to find your correct phrase. If you're sure that your known addresses and paths are correct, you can try increasing the difficulty setting to 1-4. Higher settings will take MUCH longer to run.

* `wordDistance` (default 2.0, range 0 - 10) controls the sensitivity to typos. Increasing this setting will allow more substitute words to be tested, at a substantial increase in run time. If you think you made multiple or unlikely typos in your phrase, you can try increasing this value.

* `logLevel` (default = "Info") sets the log verbosity level. From least to most verbose: `None`, `Error`, `Warning`, `Info`, `Debug` (0-5)

* `noETA` (default = false) When set to true, disables ETA calculation. If you have a very difficult phrase or passphrase to brute force, just calculating the ETA can take a long time. Setting `noETA` to true will skip this step, but you won't see a ETA on the progress meter.

# Current Limitations

* Only English BIP39 wordlist and QWERTY keyboard layouts are currently implemented
* BCH only supports legacy (1...) style addresses
* SOL deprecated derivation path `m/501'/{account}'/0/{index}` not supported


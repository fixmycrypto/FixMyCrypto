# Caution!

**Never share your recovery phrase (seed phrase, mnemonic words) with ANYONE!** Anyone asking for your recovery phrase is a scammer. Never type it into any website or any internet-connected computer or device.

# FixMyCrypto Features

* Runs totally offline so your recovery phrase is never exposed to the internet
* Automatically fixes most common mistakes which result in an "invalid recovery phrase" error:
    * Invalid words (e.g. fax -> fix)
    * Incorrect words (e.g. fix -> fox/fit/fog etc.)
    * Swapped word order
    * Missing or unknown words
* BIP39 passphrase cracking, including: typo repair, wildcards, and partial brute forcing
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
    * Need another coin? Let us know!
* Smart typo detection drastically reduces the search time
    * Phrase words are prioritized based on spelling and pronunciation similarity as well as keyboard distance (most likely typos)
* Support for special Ledger/Trezor hardware key derivation modes used for Cardano, Algorand, and Polkadot
* Simultaneous search of multiple derivation paths (including non-standard paths)
* Search a specified range of accounts and indices
* Highly multi-threaded, efficient key reuse when searching multiple paths/accounts/indices
* Blockchain search mode (requires local node & indexer)

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

# How to use

1. Download the latest release from the Releases tab (or see [`BUILD.md`](BUILD.md) to build from source)
2. Extract the .zip file to a folder
3. Copy or rename "settings.example.json" to "settings.json"
4. (Optional) Run `FixMyCrypto` to watch it run with the default included settings (using a test recovery phrase)
5. **Disconnect from the network (unplug Ethernet cable, shut off WiFi).**
6. Edit the `settings.json` file, filling in the details as described below. At a **minimum**, you must provide:
    * [Coin](#coin)
    * [Phrase](#phrase)
    * [Passphrase](#passphrase) (if used)
    * [Known address](#known-addresses) (at least one)
7. Run `FixMyCrypto`

# Checkpoints

The software will save the current progress every 30 seconds to the file `checkpoint.json` so you can resume a long running job after it gets interrupted. If this file exists when the program starts, it will resume near where it left off. If you change your settings or start a new job, you should delete `checkpoint.json` to restart from the beginning.

# Paranoid "Amnesia" Environment

If you want to be extra careful, you can run the software in an amnesiac environment which doesn't save any files to your hard drive, such as [`Tails Linux`](https://tails.boum.org). You still need to disconnect from the network before entering your phrase, but this ensures that no traces will be left behind once you reboot your system. Note that in the case of a power failure or reboot, your progress will be lost since the checkpoint file won't persist after a reboot.

---

# Configuration (settings.json file)

## Coin

### Required

Specify which cryptocurrency you are searching for. (`BTC`, `ETH`, `ADA`, `DOGE`, `LTC`, `SOL`, `ALGO`, `DOT`, `XRP`, etc.). (For ADA or DOT used with a Ledger or Trezor hardware wallet, see the relevant [special use cases](#special-use-cases) section.)

    "coin": "BTC",

If you have many coins tied to the same phrase, pick any coin for which you are sure you know the first address (account 0, index 0) which will speed up the search.

## Phrase

### Required

Enter your recovery phrase ("seed phrase" or "mnemonic phrase") between the quotation marks. The total number of words must be the same number as the length of your original recovery phrase (12, 15, 18, or 24 words for most cryptos; 25 words for `My Algo` wallets). 

    "phrase": "apple banana pear watermelon kiwi strawberry apple banana pear watermelon kiwi strawberry",

### Unknown / Invalid Words

If you know the position of a word but you have no idea what that word is supposed to be, replace it with an asterisk (`*`). For example, if you're certain that you're missing the 2nd word:

    "phrase": "apple * pear watermelon kiwi strawberry apple banana pear watermelon kiwi pear",

**Hint:** You might be tempted to replace any invalid words (words you wrote down that aren't on the BIP39 list) with a valid word from the list or a `*`, but it's actually better to leave any mistakes as-is. By leaving the invalid words in place, the software will immediately know which word(s) need to be changed first, instead of needing to check every word in the phrase. Also, the program will likely do a better job than you of guessing which typos were made and which replacement words should be tested.

Repairing up to 3 invalid / incorrect words is typically feasible, sometimes 4 if the typos aren't too bad, but each additional incorrect word will increase the search time exponentially.

### Missing Words

If you are missing some words and don't know where they go or which position(s) are missing, add a question mark (`?`) to the end of the phrase for each missing word. For example, if you only have 11 out of 12 words:

    "phrase": "apple banana pear watermelon kiwi strawberry apple banana pear watermelon kiwi ?",

A single missing word (one `?`) can be solved quickly, two missing words will take a few hours, but 3 or more missing words can take a VERY long time to solve, since the program must try every possible word in every possible position.

---

## Passphrase

### Required

A BIP39 passphrase (a.k.a. "extra word" or "advanced security") is not to be confused with your spending password, wallet password, or app login password.

If you didn't use a BIP39 passphrase when you created the wallet or you're not sure what this is, then leave this as blank:

    "passphrase": "",

If you used a BIP39 passphrase and you are 100% certain of the exact passphrase you used, specify it here:

    "passphrase": "ThePassphrase!",

If you used a BIP39 passphrase and you're 100% certain it's one from a list of possible passphrases, you can specify an array of passphrases to test:

    "passphrase": [

        "FirstPossibility",

        "SecondPossibility"
    ],

This will match a passphrase of "FirstPossibility" or "SecondPossibility" **(exact matches only)**.

## Passphrase Fuzzing

Fuzzing is used when you may have made typos in your passphrase when you created the wallet or wrote down the passphrase. Fuzzing will test all possible typo mistakes, including: insertions, deletions, substitutions, and transpositions.

To use passphrase fuzzing, place two curly braces `{{` at the start of the passphrase, and 2 at the end `}}`:

    "passphrase": "{{ThePassphrase!}}",

This will test e.g. "ThePass**hp**rase!" and all other single typos of "ThePassphrase!" (2,846 permutations).

You can fuzz multiple passphrases if you're not sure which one you used:

    "passphrase": [

        "{{FirstPossibility}}",

        "{{SecondPossibility}}
    ],

This will perform a fuzzy match of all possible typos of "FirstPossibility" and "SecondPossibility" (6,716 permutations).

If you think you have more than 1 typo in your passphrase, you can set the `fuzzDepth` setting to 2 (this increases the search time exponentially):

    "passphrase": "{{ThePassphrase!}}",

    "fuzzDepth": 2,

This will test e.g. "ThePass**hp**rase**1**" and all other single **or double** typos of "ThePassphrase!" (8,387,574 permutations).

**Brute forcing more than 2 typos in a passphrase is unlikely to succeed due to the huge number of permutations**.  If you have at least some idea as to where the typo(s) were made, then try using wildcards.

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
    * `[{]` use square brackets around a curly brace to escape it, if your passphrase happens to start AND end with 2 curly braces
    * `[?]` will escape a question mark (only needed if it comes immediately after a right square bracket or right parenthesis)
    * `^` at the start of a bracket expression means to exclude all the listed items, i.e. match any ASCII printable character except for those that are listed.
        * `[^a-zA-Z]` will match any non-letter character (matches one digit or symbol)
        * `[^a-zA-Z0-9]` will match any non-alphanumeric character
        * `[^^]` Two carets will escape a caret (matches "^"); `[^^$]` will match "^" or "$"
    * Nested square bracket expressions are not allowed
* `?` after a parenthesis or bracket expression indicates that the enclosed item is optional (i.e. it occurs zero or one times)
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

    "passphrase": "{{CorrectHorseBatteryStaple42!}}",

This would match all possible single typos:
* "CorrectHorseBatterySta**b**le42!"
* "Co**r**ectHorseBatteryStaple42!"
* "CorrectHo**o**rseBatteryStaple42!", etc. (5,793 permutations)

### Example 4

You're certain your passphrase was either "MyUsualPassword", "MyOtherPassword", or "4321", but none of those works. You can try fuzzing all of them simultaneously:

    "passphrase": [

        "{{MyUsualPassword}}",

        "{{MyOtherPassword}},

        "{{4321}}"
    ],

This will match typos of any of those passphrases, e.g.:
* "MyUsualPassw**a**rd"
* "MyOt**t**erPassword"
* "4**23**1", etc. (6961 permutations)

---

## Known Addresses

### Strongly Recommended

Provide 1 or more addresses that you are certain belong to this wallet. Ideally, you should put the first address that was created by the wallet (account 0, index 0). If you aren't sure which address is address 0, provide as many addresses as you can. Adding more addresses here doesn't slow the search down, in fact it is more likely to speed things up. Check your transaction history with exchanges, wallets, e-mail receipts, etc. to find the receive address(es) you used with this wallet. If you don't know ANY of your addresses, please see below for further instructions on using blockchain search mode.

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

This specifies the derivation path(s) used to generate addresses. Most users can leave this blank to search using default derivation paths. In most cases we can detect the path from the address format or the default paths used by the coin. However, if you used a particularly old wallet software or one known to use a non-standard derivation path, it will be necessary to specify it here. You can specify one or more paths. The more paths you specify, the longer the search will take. Use `{account}` as the placeholder for the account number, and `{index}` as the placeholder for the address index.

### Example

If you used old versions of Ledger or Coinomi to generate legacy BTC addresses which start with "1…", you may need to specify this non-standard path:

    "paths": [
        "m/44'/0'/{account}'/{index}"
    ],

Note that the search will fail if none of the paths you specify match the one used to generate your wallet addresses.

---
## Special use cases

### Ethereum

If you used a Ledger hardware wallet to generate an Ethereum address around 2019 or prior, you may need to specify the non-standard derivation path `m/44'/60'/{account}'/{index}`, as opposed to the standard path `m/44'/60'/{account}'/0/{index}` (note the extra 0 between the account & index, which is for specifying external vs internal change addresses, but was missing from older Ledger versions).

You can search both paths at the same time like this (but the search will take a bit longer):

    "paths": [
        "m/44'/60'/{account}'/0/{index}",

        "m/44'/60'/{account}'/{index}"
    ],

If you used older versions of Metamask together with a Ledger hardware wallet, some users noted that it used an incorrect account value (e.g. 10) instead of 0 to generate addresses. In this case you can specify the account range to search as `"0,10"` (only accounts 0 and 10), or `"0-10"` (accounts 0 through 10). It's possible that other account numbers may have been used as well, so you may need to specify a larger range. Note that searching more account numbers increases the search time.

    "accounts": "0-10",

### Ethereum Classic (ETC)

Use "ETH" for the coin type, and use one or more of the following paths:

* `m/44'/60'/{account}'/0/{index}` – Pre-split ETH/ETC wallet
* `m/44'/61'/{account}'/0/{index}` – Post-split ETC wallet
* `m/44'/60'/160720'/0/{index}` – Ledger ETC
* `m/44'/60'/160720'/0'/{index}` – Old Ledger, Vintage MEW

See https://medium.com/myetherwallet/hd-wallets-and-derivation-paths-explained-865a643c7bf2 for other possible paths used by ETC and other ETH-derived coins. You can search multiple paths at the same time if you aren't sure which one to use.

### Cardano (ADA)

Ledger & Trezor hardware wallets use unique methods to convert your recovery phrase into a master private key, which is not compatible with the official Cardano key spec used by software wallets like Daedalus, Yoroi, and Adalite. This means the same recovery phrase will create a different wallet (different addresses) depending on whether you use the phrase on a Ledger, Trezor, or software wallet. If you used a Ledger or Trezor wallet for Cardano and need to recover it, use the coin type `ADATrezor` or `ADALedger` to recover your address, otherwise the addresses won't match and recovery won't be possible.

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

# Blockchain Search Mode

### Advanced users only

Blockchain search mode can be used if you have a partially valid recovery phrase but don't know any of the addresses associated with this phrase. For example, if your computer was lost or stolen and you don't have any transaction logs to refer to.

## Blockchain search requirements

* Locally running up-to-date node with a fully complete initial block download
    * This allows the search to be performed offline
    * Ideally you don't want to use an online indexer API since they require internet access and limit or throttle requests unless you pay for API access
* Supported indexers:
    * BTC: Mempool + Electrs (in theory bitcore should work, but it's too slow to index BTC)
    * BCH, DOGE, LTC, other BTC-alts: bitcore
    * ETH: geth (serves as both the node and indexer)
    * ADA: cardano-graphql (docker-compose runs both the node and the indexer)
    * SOL/ALGO/DOT/XRP: (not yet supported)
* Refer to node/indexer documentation for setting up the node
* Allow IBD and index to complete, then take system offline before entering recovery phrase & starting the search.

To use blockchain search mode, leave the knownAddresses field blank. You must specify the API path in the settings.json file:

    "adaApi": "http://localhost:3100", "adaApiType": "graphql",

    "btcApi": "http://localhost:8081", "btcApiType": "mempool",

    "ethApi": "http://localhost:8545", "ethApiType": "gethrpc",

    "altcoinApi": "http://localhost:3000", "altcoinApiType": "bitcore",

# Current Limitations

* Only English BIP39 wordlist and QWERTY keyboard layouts are currently implemented
* BCH only supports legacy (1...) style addresses
* SOL deprecated derivation path `m/501'/{account}'/0/{index}` not supported
* SOL/ALGO/DOT/XRP blockchain search not implemented


# Caution!

**Never share your recovery phrase (seed phrase, mnemonic words) with ANYONE!** Anyone asking for your recovery phrase is a scammer. Never type it into any website or any internet-connected computer or device.

# FixMyCrypto Features

* Automatically fixes most common mistakes which result in an "invalid recovery phrase" error:
    * Invalid words (e.g. fax -> fix)
    * Valid but incorrect words (e.g. fix -> fox/fit/fog etc.)
    * Swapped word order
    * Missing words
* BIP39 passphrase support, including wildcards / brute forcing
* Runs totally offline so your recovery phrase is never exposed to the internet
* Smart typo detection drastically reduces the search time
    * Words are prioritized based on spelling and pronunciation similarity as well as keyboard distance (most likely typos)
* Coin support: 
    * BTC (+ forks e.g. BCH, etc.)
    * ETH (+ forks e.g. ETC)
    * LTC
    * DOGE
    * ADA (Cardano) - including special mode for recovering hardware Ledger/Trezor wallets
    * SOL
* Simultaneous search of multiple derivation paths (including non-standard paths)
* Search a specified range of accounts and indices
* Highly multi-threaded, efficient key reuse when searching multiple paths/accounts
* Blockchain search mode (requires local node & indexer)

# Support

For technical support or help with any crypto recovery needs, please contact us via [e-mail](mailto:help@fixmycrypto.com) or via our website <https://www.fixmycrypto.com>

# Donations / Tips

If you found this software useful, please consider donating to fund future development.

* BTC: bc1q477afku8x7964gmzlsapgj8705e63ch89p8k4z
* ETH: 0x0327DF6652D07eE6cc670626b034edFfceD1B20C
* DOGE: DT8iZF8RbqpRftgrWdiq34EZdJpCGiWBwG
* ADA: addr1qxhjru35kv8fq66afxxdnjzf720anfcppktchh6mjuwxma3e876gh3czzkq0guls5qrkghexsuh543h7k2xqlje5lskqfp2elv

# Build

See BUILD.md

# Usage

* **Disconnect from the network (unplug Ethernet cable, shut off WiFi).**
* Copy or rename "settings.example.json" to "settings.json"
* Edit the `settings.json` file, filling in the details as described below.
* Run `bin/Debug/net5.0/FixMyCrypto` or `bin/Release/net5.0/(platform)/publish/FixMyCrypto` (or load the project is VS Code and use the Run configuration)

# Configuration (settings.json file):

## Coin:

### Required

Specify which cryptocurrency you are searching for. (`BTC`, `ETH`, `ADA`, `DOGE`, `LTC`, `SOL`, etc.). (For ADA used with a Ledger or Trezor hardware wallet, see "Cardano special use cases" below.)

    "coin": "BTC",

## Phrase:

### Required

Enter your recovery phrase between the quotation marks. The number of words must be the same number as the length of your original recovery phrase (12, 15, 18, or 24). If you don't know all the words, guess whichever words you don't know (add words to the end until you get to the correct number of words). Try your best to keep the words you know in their original order!

    "phrase": "apple banana pear watermelon kiwi strawberry apple banana pear watermelon kiwi strawberry",

Hint: You might be tempted to replace any invalid words (words not on the BIP39 list) with valid words from the list, but it's actually better to leave any mistakes as-is. First, by leaving the invalid words in place, the software will immediately know which word(s) need to be changed first, instead of needing to try every word in the phrase. Second, the program may do a better job than you of guessing which typos were made and which replacement words should be tested.

Repairing up to 3 invalid / incorrect words is typically feasible, sometimes 4 if the typos aren't too bad, but each additional incorrect word will increase the search time exponentially.

## Passphrase:

### Required

If you used a "BIP39 passphrase" (a.k.a. "extra word", "25th word", or "advanced security", NOT your wallet password or spending password) when you created the wallet, try to provide the exact passphrase that you used:

    "passphrase": "ThePassphrase!",

If your passphrase contains left (opening) square brackets `[` or left (opening) parenthesis `(` then you must escape these characters by putting an extra set of square brackets around them, see below.

If you have a pretty good but not exact idea of what the passphrase is, you can use the following wildcards. Keep in mind that each wildcard used will increase the search time exponentially. **Brute forcing the entirety of a long passphrase is not feasible.**

* If you know part of the passphrase is one of a range of characters, put the range in square brackets with a hyphen between the values, similar to regex expressions.
    * `[a-z]` will match one lower case letter a-z
    * `[a-zA-Z]` will match one lower or upper case letter a-z or A-Z
    * `[aeiou]` will match one lower case vowel letter, e.g. "a"
    * `[0-9]` will match one digit
    * `[!@#$%^&*()]` will match one of the listed special symbols
    * `[[]`, `[]]`, `[(]`, `[)]` will escape a left/right square bracket or left/right parenthesis, respectively
    * `[?]` will escape a question mark (only needed if it comes immediately after a right square bracket or right parenthesis)
* If you know part of the passphrase is one of two or more options, you can put that part in parenthesis using a vertical bar `|` as the "or" separator, i.e.:
    * `(T|t)he` will match "The" or "the"
    * `(Correct|Horse|Battery)` will match "Correct", "Horse", or "Battery"
* A parenthesis or bracket expression can be followed by a question mark "?" to indicate that the enclosed item is optional (i.e. it occurs zero or one times)
    * `(T|t)?he` will match "The", "the", or "he"
    * `Hello Dolly[!$]?` will match "Hello Dolly", "Hello Dolly!", or "Hello Dolly$"
    * `[0-9][0-9]?` will match any one or two digit number (0 - 99)
* Nested search terms are not supported

For example, let's say your passphrase contains the words "hello" and "dolly" (with uncertain capitalization), followed by one symbol out of `!@#$%^&*`, and finally one or two digits, you would specify:

    "passphrase": "(H|h)ello(D|d)olly[!@#$%^&*][0-9][0-9]?",

This would match "hellodolly!1", "HelloDolly$42", "Hellodolly*69", etc.

## Known Addresses:

### Strongly Recommended

Provide 1 or more addresses that you are certain belong to this wallet. Ideally, you should put the first address that was created by the wallet (address 0). If you aren't sure which address is address 0, provide as many addresses as you can. Adding more addresses here doesn't slow the search down, in fact it is more likely to speed things up. Check your transaction history with exchanges, wallets, e-mail receipts, etc. to find the receive address(es) you used with this wallet. If you don't know ANY of your addresses, please see below for further instructions on using blockchain search mode.

In the case of Ethereum, only one address (index 0) is typically used because the same address is used for every transaction on the same account.

If you only know one address, it should be filled out like this:

    "knownAddresses":  [
        "bc1qmr9cmy3p3q695mkn5rnmz27csvnvr2ja05wa4p"
    ],

If you know more than one address, it should look like this:

    "knownAddresses":  [
            "bc1qmr9cmy3p3q695mkn5rnmz27csvnvr2ja05wa4p",

            "1BzeytZbmdDt4678V4MnCEp2m2bM42Ey4p",

            "3A7Wg827LsxcEiaMmpeaKoKMGPfUfCGH4X",
        ],

## Indices:

### Recommended

"indices" specifies the range of address indices to check against. UTXO-based blockchains such as Bitcoin and Cardano use a different address index (new address) for each transaction. Each index represents one address from the wallet. 

The default setting is to check if the known address(es) are in the first 5 addresses of the wallet, hence `"0-4"`. This means one of the known addresses you provide must belong to one of the first five transactions received by this wallet. If you know that your address is between index 5 and 10, use `"5-10"`. Ranges can be specified using hyphens: e.g. `"0-5"`, commas: e.g. `"2,4,6"`, or a mix of both e.g.: `"0,2,4,10-12"`. The more indices you search, the longer it will take. Ideally, if you know you have provided your address 0, then you can set this to `"0"` to speed things up a bit. However, if your known address is address index 5, and you specify the range as `"0-4"`, it won't find your address at all. So make sure this range is big enough to include at least one of the known addresses you've provided.

For ETH and SOL, typically only address index 0 is used, so you should change this to `"0"` to speed things up. Your other ETH or SOL addresses will typically belong to a different account number instead of a different index, see below.

## Accounts:

### Recommended

Similar to indices, "accounts" specifies the range of accounts to check against. For most users, if you had only one account for a particular coin tied to this recovery phrase, the account should be set to `"0"`. As with "indices", you can specify a range using hyphens and commas.

ETH and SOL tend to use different account numbers instead of different indices.

## Paths:

### Optional (usually)

This specifies the derivation path(s) used to generate addresses. Most users can leave this blank to search using default derivation paths. In most cases we can detect the path from the address format or the default paths used by the coin. However, if you used a particularly old wallet software or one known to use a non-standard derivation path, it will be necessary to specify it here. You can specify one or more paths. The more paths you specify, the longer the search will take. Use `{account}` as the placeholder for the account number, and `{index}` as the placeholder for the address index.

Examples:

If you used old versions of Ledger or Coinomi to generate legacy BTC addresses which start with "1…", you may need to specify this non-standard path:

    "paths": [
        "m/44'/0'/{account}'/{index}"
    ],

Note that the search will fail if none of the paths you specify match the one used to generate your wallet addresses.

## Ethereum special use cases:

If you used a Ledger hardware wallet to generate an Ethereum address around 2019 or prior, you may need to specify the non-standard derivation path `m/44'/60'/{account}'/{index}`, as opposed to the standard path `m/44'/60'/{account}'/0/{index}` (note the extra 0 between the account & index, which is for specifying external vs internal change addresses, but was missing from older Ledger versions).

You can search both paths at the same time like this (but the search will take a bit longer):

    "paths": [
        "m/44'/60'/{account}'/0/{index}",

        "m/44'/60'/{account}'/{index}"
    ],

If you used older versions of Metamask together with a Ledger hardware wallet, some users noted that it used an incorrect account value (e.g. 10) instead of 0 to generate addresses. In this case you can specify the account range to search as `"0,10"` (only accounts 0 and 10), or `"0-10"` (accounts 0 through 10). It's possible that other account numbers may have been used as well, so you may need to specify a larger range. Note that searching more account numbers increases the search time.

    "accounts": "0-10",

## Ethereum Classic (ETC):

Use "ETH" for the coin type, and use one or more of the following paths:

* `m/44'/60'/{account}'/0/{index}` – Pre-split ETH/ETC wallet
* `m/44'/61'/{account}'/0/{index}` – Post-split ETC wallet
* `m/44'/60'/160720'/0/{index}` – Ledger ETC
* `m/44'/60'/160720'/0'/{index}` – Old Ledger, Vintage MEW

See https://medium.com/myetherwallet/hd-wallets-and-derivation-paths-explained-865a643c7bf2 for other possible paths used by ETC and other ETH-derived coins. You can search multiple paths at the same time if you aren't sure which one to use.

## Cardano special use cases:

Ledger & Trezor hardware wallets use unique methods to convert your recovery phrase into a master private key, which is not compatible with the official Cardano key spec used by software wallets like Daedalus, Yoroi, and Adalite. This means the same recovery phrase will create a different wallet (different addresses) depending on whether you use the phrase on a Ledger, Trezor, or software wallet. If you used a Ledger or Trezor wallet for Cardano and need to recover it, use the coin type `ADATrezor` or `ADALedger` to recover your address, otherwise the addresses won't match and recovery won't be possible.

    "coin": "ADALedger",

## Other settings:

* `difficulty` (default 0, range 0-4) controls which algorithms (different types of word substitutions, swaps, etc.) will be tested. Leave this at default unless the program fails to find your correct phrase. If you're sure that your known addresses and paths are correct, you can try increasing the difficulty setting to 1-4. Higher settings will take MUCH longer to run.

* `wordDistance` (default 2.0, range 0 - 10) controls the sensitivity to typos. Increasing this setting will allow more substitute words to be tested, at a substantial increase in run time. If you think you made multiple or unlikely typos in your phrase, you can try increasing this value.

* `logLevel` (default = "Warning") sets the log verbosity level. From least to most verbose: `None`, `Error`, `Warning`, `Info`, `Debug` (0-5)

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
    * SOL: (not yet supported)
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
* SOL blockchain search not implemented


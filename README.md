# Caution!

**Never share your recovery phrase (seed phrase, mnemonic words) with ANYONE!** Anyone asking for your recovery phrase is a scammer. Never type it into any website or any internet-connected computer or device.

# FixMyCrypto Features

* Automatically fixes most common mistakes which result in the "invalid recovery phrase" error
    * Invalid words (e.g. fax -> fix)
    * Valid but incorrect words (e.g. fix -> fox/fit/fog etc.)
    * Swapped word order
* Runs totally offline so your recovery phrase is never exposed to the internet
* Smart typo detection drastically reduces the search time
* Coin support: 
    * BTC (+ forks e.g. BCH, etc.)
    * ETH (+ forks e.g. ETC)
    * LTC
    * DOGE
    * ADA (Cardano) - including special mode for recovering hardware Ledger/Trezor wallets
* Simultaneous search of multiple derivation paths (including non-standard paths)
* Search a specified range of accounts and indices
* Highly multi-threaded, efficient key reuse when searching multiple paths/accounts
* BIP39 passphrase support
* Blockchain search mode (requires local node & indexer)

# Build

See BUILD.md

# Usage

* **Disconnect from the network (unplug Ethernet cable, shut off WiFi).**
* Copy or rename “settings.example.json” to “settings.json”
* Edit the settings.json file, filling in the details as described below.
* Run bin/Debug/net5.0/FixMyCrypto or bin/Release/net5.0/(platform)/publish/FixMyCrypto

# Configuration (settings.json file):

## Coin:

### Required

Specify which cryptocurrency you are searching for. (“BTC”, “ETH”, “ADA”, “DOGE”, “LTC”, etc.). (For ADA used with a hardware wallet, see “Cardano special use cases” below.)

    "coin": "BTC",

## Phrase:

### Required

Enter your recovery phrase between the quotation marks. The number of words must be the same number as the length of your original recovery phrase (12, 15, 18, or 24). If you don’t know all the words, guess whichever words you don’t know. But try your best to keep the words in the original order!

    "phrase": "apple banana pear watermelon kiwi strawberry apple banana pear watermelon kiwi strawberry",

Hint: You might be tempted to fix any invalid words (words not on the BIP39 list), but it is better to leave any mistakes as-is. First, by leaving the invalid words in place, the software will immediately know which word(s) need to be changed, instead of trying every word in the phrase. Second, the program will probably do a better job of guessing which typos were made.

## Known Addresses:

### Recommended

“knownAddresses” is very important. Here, you should put at least 1 or more addresses that you are certain belong to this wallet. Ideally, you should put the first address that was created by the wallet (address 0). If you aren’t sure which address is address 0, put as many addresses in here as you can. Adding more addresses here does NOT slow the search down, in fact it is more likely to speed things up. Check your transaction history with exchanges, wallets, e-mail receipts, etc. to find the receive address(es) you used with this wallet. If you don’t know ANY of your addresses, please contact us for further instructions on using blockchain search mode.

In the case of Ethereum, only one address (index 0) is typically used because the same address is used for every transaction.

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

## Indices and Accounts:

### Recommended

“indices” specifies the range of address indices to check against. By default we search to see if any of the known addresses are in the first 5 addresses of the wallet, hence “0-4”. This means at least one of the known addresses you provide must be one of the first five addresses used with this wallet. If you know that your address is between index 5 and 10, use “5-10”. Ranges can be specified using hyphens: e.g. “0-5”, commas: e.g. “2,4,6”, or a mix of both e.g.: “0,2,4,10-12”. The more addresses you search, the longer it will take. Ideally, if you know you have provided your address 0, then you can set this to “0” to speed things up a bit. However, if your known address is address index 5, and you specify the range as “0-4”, it won’t find your address at all. So make sure this range is big enough to include at least one of the known addresses you’ve provided.

For ETH, typically only address index "0" is used.

Similarly, “accounts” specifies the range of accounts to check against. For most users, if you had only one account for a particular coin tied to this recovery phrase, the account will be “0”. As with “indicies”, you can specify a range using hyphens and commas.

## Paths:

### Optional (usually)

This specifies the derivation path(s) used to generate addresses. Most users can leave this blank to search using default derivation paths. In most cases we can detect the path from the address format or the default paths used by the coin. However, if you used a particularly old wallet software or one known to use a non-standard derivation path, it will be necessary to specify it here. You can specify one or more paths. The more paths you specify, the longer the search will take. Use “{account}” as the placeholder for the account number, and “{index}” as the placeholder for the address index.

Examples:

If you used old versions of Ledger or Coinomi to generate legacy BTC addresses which start with “1…”, you may need to specify this non-standard path:

    "paths": [
        "m/44'/0'/{account}'/{index}"
    ],

Note that the search will fail if none of the paths you specify matches the one used to generate your wallet addresses.

## Ethereum special use cases:

If you used a Ledger hardware wallet to generate an Ethereum address around 2019 or prior, you may need to specify the non-standard derivation path “m/44’/60’/{account}’/{index}”, as opposed to the standard path “m/44’/60’/{account}’/0/{index}” (note the extra 0 between the account & index, which is for specifying external vs internal change addresses, but was missing from older Ledger versions).

You can search both paths at the same time like this (but the search will take a bit longer):

    "paths": [
        "m/44'/60'/{account}'/0/{index}",

        "m/44'/60'/{account}'/{index}"
    ],

If you used older versions of Metamask together with a Ledger hardware wallet, some users noted that it used an incorrect account value (e.g. 10) instead of 0 to generate addresses. In this case you can specify the account range to search as “0,10” (only accounts 0 and 10), or “0-10” (accounts 0 through 10). It’s possible that other account numbers may have been used as well, so you may need to specify a larger range. Note that searching more account numbers increases the search time.

    "accounts": "0-10",

## Ethereum Classic (ETC):

Use “ETH” for the coin type, and use one or more of the following paths:

* m/44’/60’/{account}’/0/{index} – Pre-split ETH/ETC wallet
* m/44’/61’/{account}’/0/{index} – Post-split ETC wallet
* m/44’/60’/160720’/0/{index} – Ledger ETC
* m/44’/60’/160720’/0’/{index} – Old Ledger, Vintage MEW

See https://medium.com/myetherwallet/hd-wallets-and-derivation-paths-explained-865a643c7bf2 for other possible paths used by ETC and other ETH-derived coins.

## Cardano special use cases:

Ledger & Trezor hardware wallets use unique methods to convert your recovery phrase into a master private key, which is not compatible with the official Cardano key spec used by software wallets like Daedalus, Yoroi, and Adalite. This means the same recovery phrase will create a different wallet (different addresses) depending on whether you use the phrase on a Ledger, Trezor, or software wallet. If you used a Ledger or Trezor wallet for Cardano and need to recover it, use the coin type “ADATrezor” or “ADALedger” to recover your address, otherwise the addresses won’t match and recovery won’t be possible.

    "coin": "ADALedger",

## Other settings:

* If you used a “BIP39 passphrase” (a.k.a. “25th word”, NOT your wallet password or spending password) when you created the wallet, put it between the quotes next to “passphrase”. Note: If you used a passphrase, you must provide the EXACT passphrase used. The program doesn’t currently attempt to guess or change the passphrase, but this may be added to a future version.

* The other settings can generally be left alone.

# Support

For technical support or help with any crypto recovery needs, please contact us via [e-mail](mailto:help@fixmycrypto.com) or via our commercial site <https://www.fixmycrypto.com>

# Donations / Tips

If you found this software useful, please consider donating to fund future development.

* BTC: bc1q477afku8x7964gmzlsapgj8705e63ch89p8k4z
* ETH: 0x0327DF6652D07eE6cc670626b034edFfceD1B20C
* DOGE: DT8iZF8RbqpRftgrWdiq34EZdJpCGiWBwG
* ADA: addr1qxhjru35kv8fq66afxxdnjzf720anfcppktchh6mjuwxma3e876gh3czzkq0guls5qrkghexsuh543h7k2xqlje5lskqfp2elv

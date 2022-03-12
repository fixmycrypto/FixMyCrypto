namespace FixMyCrypto {
    class Bip39_Solver {
        public static string int_to_address_cl = @"

#define inBufferSize <inBufferSize_bytes>
#define outBufferSize <outBufferSize_bytes>
#define saltBufferSize <saltBufferSize_bytes>
//#define pwdBufferSize <pwdBufferSize_bytes>
#define hashlength 64

    typedef struct {
        <word_type> length; // in bytes
        uchar buffer[inBufferSize];
    } inbuf;

    typedef struct {
        uchar buffer[outBufferSize];
    } outbuf;

    // Salt buffer, used by pbkdf2 & pbe
    typedef struct {
        <word_type> length; // in bytes
        uchar buffer[saltBufferSize];
    } saltbuf;

    void copy_pad_previous(uchar *pad, uchar *previous, uchar *joined) {
        for(int x=0;x<128;x++){
            joined[x] = pad[x];
        }
        for(int x=0;x<hashlength;x++){
            joined[x+128] = previous[x];
        }
    }

    void xor_seed_with_round(__global uchar *seed, uchar *round) {
        for(int x=0;x<hashlength;x++){
            seed[x] = seed[x] ^ round[x];
        }
    }

    void print_byte_array_hex(uchar *arr, int len) {
        for (int i = 0; i < len; i++) {
            printf(""%02x"", arr[i]);
        }
        printf(""\n\n"");
    }

/*
void print_wg_size(__constant const char *name) {
    size_t g = get_global_size(0);
    size_t l = get_local_size(0);
    size_t wg = get_num_groups(0);
    printf(""%s(): global=%ld local=%ld groups=%ld\n"", name, g, l, wg);
}
*/

    __kernel void pbkdf2(__global inbuf *inbuffer, __global const saltbuf *saltbuffer, __global outbuf *outbuffer,
    __private unsigned int iters, __private unsigned int dkLen_bytes, uchar mode) {

        ulong idx = get_global_id(0);
        uchar ipad_key[128];
        uchar opad_key[128];
        uchar pwd_hash[hashlength] = { 0 };

/*
        //  debug
        if (idx == 0)
        {
            print_wg_size((__constant char*)""pbkdf2 (bip39)"");
        }
*/

        __global uchar *pwd;
        __global uchar *seed = outbuffer[idx].buffer;
        __global uchar *salt;
        <word_type> pwdLen;
        <word_type> saltLen;

        if (mode == 0)
        {
            pwd = inbuffer[idx].buffer;
            salt = saltbuffer[0].buffer;
            pwdLen = inbuffer[idx].length;
            saltLen = saltbuffer[0].length;
        }
        else
        {
            pwd = inbuffer[0].buffer;
            salt = saltbuffer[idx].buffer;
            pwdLen = inbuffer[0].length;
            saltLen = saltbuffer[idx].length;
        }

        printf(""pwd (%d): %s\n"", pwdLen, pwd);
        printf(""salt (%d): %s\n"", saltLen, salt);

        int blocks = outBufferSize / hashlength;
        //printf(""dkLen_bytes=%d outBufferSize=%d blocks=%d\n"", dkLen_bytes, outBufferSize, blocks);

        if (pwdLen > 128) {
            printf(""idx=%d start sha512\n"", idx);
            sha512(&pwd, pwdLen, &pwd_hash);
            //printf(""hashed password:"");
            //print_byte_array_hex(pwd_hash, hashlength);
            //printf(""\n"");
        }

        for (int block = 1; block <= blocks; block++) {
            for(int x=0;x<128;x++){
                ipad_key[x] = 0x36;
                opad_key[x] = 0x5c;
            }

            if (pwdLen > 128) {
                for(uint x=0;x<hashlength;x++){
                    ipad_key[x] = ipad_key[x] ^ pwd_hash[x];
                    opad_key[x] = opad_key[x] ^ pwd_hash[x];
                }
            }
            else {
                for(uint x=0;x<pwdLen;x++){
                    ipad_key[x] = ipad_key[x] ^ pwd[x];
                    opad_key[x] = opad_key[x] ^ pwd[x];
                }
            }

            uchar sha512_result[hashlength] = { 0 };
            uchar key_previous_concat[256] = { 0 };
            int x = 0;
            for(;x<128;x++){
                key_previous_concat[x] = ipad_key[x];
            }
            for(int i=0;i<saltLen;i++){
                key_previous_concat[x++] = salt[i];
            }
            key_previous_concat[x++] = (block >> 24) & 0xff;
            key_previous_concat[x++] = (block >> 16) & 0xff;
            key_previous_concat[x++] = (block >> 8) & 0xff;
            key_previous_concat[x++] = (uchar)block & 0xff;

            sha512(&key_previous_concat, x, &sha512_result);
            copy_pad_previous(&opad_key, &sha512_result, &key_previous_concat);
            sha512(&key_previous_concat, 192, &sha512_result);
            xor_seed_with_round(seed, &sha512_result);

            for(int x=1;x<iters;x++){
                //printf(""iter %d\n"", x);
                copy_pad_previous(&ipad_key, &sha512_result, &key_previous_concat);
                sha512(&key_previous_concat, 192, &sha512_result);
                copy_pad_previous(&opad_key, &sha512_result, &key_previous_concat);
                sha512(&key_previous_concat, 192, &sha512_result);
                xor_seed_with_round(seed, &sha512_result);
            }

            seed += hashlength;
        }
    }

    __kernel void pbkdf2_2048_64(__global inbuf *inbuffer, __global const saltbuf *saltbuffer, __global outbuf *outbuffer) {
        pbkdf2(inbuffer, saltbuffer, outbuffer, 2048, 64, 0);
    }

    __kernel void pbkdf2_saltlist_2048_64(__global inbuf *inbuffer, __global const saltbuf *saltbuffer, __global outbuf *outbuffer) {
        pbkdf2(inbuffer, saltbuffer, outbuffer, 2048, 64, 1);
    }

   __kernel void pbkdf2_4096_96(__global inbuf *inbuffer, __global const saltbuf *saltbuffer, __global outbuf *outbuffer) {
        pbkdf2(inbuffer, saltbuffer, outbuffer, 4096, 96, 0);
    }

    __kernel void pbkdf2_saltlist_4096_96(__global inbuf *inbuffer, __global const saltbuf *saltbuffer, __global outbuf *outbuffer) {
        pbkdf2(inbuffer, saltbuffer, outbuffer, 4096, 96, 1);
    }

        ";

        public static string address_cl = @"
#define BITCOIN_MAINNET 0
#define BITCOIN_TESTNET 1

typedef struct {
  bool compressed;
  int network;
  uchar key[32];
} private_key_t;

typedef struct {
  bool compressed;
  secp256k1_pubkey key;
} public_key_t;

typedef struct {
  uchar network;
  uchar depth;
  uchar parent_fingerprint[4];
  uint child_number;
  uchar chain_code[32];
  private_key_t private_key;
} extended_private_key_t;

typedef struct {
  uchar network;
  uchar depth;
  uchar parent_fingerprint[4];
  uint child_number;
  uchar chain_code[32];
  public_key_t public_key;
} extended_public_key_t;

void hmac_sha512(uchar *key, int key_length_bytes, uchar *message, int message_length_bytes, uchar *output) {
  uchar ipad_key[128];
  uchar opad_key[128];
  for(int x=0;x<128;x++){
    ipad_key[x] = 0x36;
    opad_key[x] = 0x5C;
  }

  for(int x=0;x<key_length_bytes;x++){
    ipad_key[x] = ipad_key[x] ^ key[x];
    opad_key[x] = opad_key[x] ^ key[x];
  }

  uchar inner_concat[256] = { 0 };

  for(int x=0;x<128;x++){
    inner_concat[x] = ipad_key[x];
  }
  for(int x=0;x<message_length_bytes;x++){
    inner_concat[x+128] = message[x];
  }

  sha512(&inner_concat, 128+message_length_bytes, output);

  for(int x=0;x<128;x++){
    inner_concat[x] = opad_key[x];
  }
  for(int x=0;x<64;x++){
    inner_concat[x+128] = output[x];
  }

  sha512(&inner_concat, 192, output);
}

void new_master_from_seed(uchar network, uchar *seed, extended_private_key_t * master) {
  uchar key[12] = { 0x42, 0x69, 0x74, 0x63, 0x6f, 0x69, 0x6e, 0x20, 0x73, 0x65, 0x65, 0x64 };
  uchar hmacsha512_result[64] = { 0 };
  hmac_sha512(&key, 12, seed, 64, &hmacsha512_result);
  private_key_t pkey;
  pkey.compressed = false;
  pkey.network = network;
  memcpy_offset(&pkey.key, &hmacsha512_result, 0, 32);
  master->network = network;
  master->depth = 0;
  master->parent_fingerprint[0] = 0x00;
  master->parent_fingerprint[1] = 0x00;
  master->parent_fingerprint[2] = 0x00;
  master->parent_fingerprint[3] = 0x00;
  master->child_number = 0;
  master->private_key = pkey;
  memcpy_offset(&master->chain_code, &hmacsha512_result, 32, 32);
}

void public_from_private(extended_private_key_t *priv, extended_public_key_t *pub) {
  pub->network = priv->network;
  pub->depth = priv->depth;
  pub->child_number = priv->child_number;
  memcpy(&pub->parent_fingerprint,&priv->parent_fingerprint, 4);
  memcpy(&pub->chain_code, &priv->chain_code, 32);
  secp256k1_ec_pubkey_create(&pub->public_key.key, &priv->private_key.key);
}

void serialized_public_key(extended_public_key_t *pub, uchar *serialized_key) {
  secp256k1_ec_pubkey_serialize(serialized_key, 33, &pub->public_key.key, SECP256K1_EC_COMPRESSED);
}

void uncompressed_public_key(extended_public_key_t *pub, uchar *serialized_key) {
  secp256k1_ec_pubkey_serialize(serialized_key, 65, &pub->public_key.key, SECP256K1_EC_UNCOMPRESSED);
}

void sha256d(uchar *input, int input_len,char * output) {
  sha256((__private uchar*)input, input_len, (__private char*)output);
  sha256((__private char*)output, 32, (__private char*)output);
}

void hash160(uchar *input, int input_len, char * output) {
  uchar sha256_result[32] = { 0 };
  sha256((__private uchar*)input, input_len, (__private uchar*)&sha256_result);
  ripemd160(&sha256_result, 32, output);
}

void identifier_for_public_key(extended_public_key_t *pub, uchar *identifier) {
  uchar serialized_key[33] = {0};
  serialized_public_key(pub, &serialized_key);
  hash160(&serialized_key, 33, identifier);
}

void fingerprint_for_public_key(extended_public_key_t *pub, uchar *fingerprint) {
  uchar identifier[20] = { 0 };
  identifier_for_public_key(pub, &identifier);
  fingerprint[0] = identifier[0];
  fingerprint[1] = identifier[1];
  fingerprint[2] = identifier[2];
  fingerprint[3] = identifier[3];
}

void p2shwpkh_address_for_public_key(extended_public_key_t *pub, uchar *address_bytes) {
  uchar pubkey_hash[20] = { 0 };
  identifier_for_public_key(pub, &pubkey_hash);

  uchar wpkh_script_bytes[22] = { 0 };
  wpkh_script_bytes[0] = 0x00; // version byte
  wpkh_script_bytes[1] = 0x14; // hash160 length of 20
  for(int i=0;i<20;i++){
    wpkh_script_bytes[i+2] = pubkey_hash[i]; // hash160(pubkey)
  }

  uchar wpkh_script_hash[20] = { 0 };
  hash160(&wpkh_script_bytes, 22, &wpkh_script_hash);

  address_bytes[0] = 5; // bitcoin mainnet; 196 for testnet

  for(int i=0;i<20;i++) {
    address_bytes[i+1] = wpkh_script_hash[i];
  }
  
  uchar sha256d_result[32] = { 0 };
  sha256d(address_bytes, 21, &sha256d_result);

  // append checksum
  address_bytes[21] = sha256d_result[0];
  address_bytes[22] = sha256d_result[1];
  address_bytes[23] = sha256d_result[2];
  address_bytes[24] = sha256d_result[3];
}

void normal_private_child_from_private(extended_private_key_t *parent, extended_private_key_t *child, uint normal_child_number) {
  uchar hmacsha512_result[64] = { 0 };
  extended_public_key_t pub;
  public_from_private(parent, &pub);
  uchar hmac_input[37] = {0};
  serialized_public_key(&pub, &hmac_input);
  hmac_input[33] = normal_child_number >> 24;
  hmac_input[34] = (normal_child_number & 0x00FF0000) >> 16;
  hmac_input[35] = (normal_child_number & 0x0000FF00) >> 8;
  hmac_input[36] = (normal_child_number & 0x000000FF);
  hmac_sha512(&parent->chain_code, 32, &hmac_input, 37, &hmacsha512_result);

  private_key_t sk;
  sk.compressed = true;
  sk.network = parent->network;
  memcpy(&sk.key, &hmacsha512_result, 32);
  secp256k1_ec_seckey_tweak_add(&sk.key, &parent->private_key.key);
  child->network = parent->network;
  child->depth = parent->depth + 1;
  child->child_number = normal_child_number;
  child->private_key = sk;
  memcpy_offset(&child->chain_code, &hmacsha512_result, 32, 32);
}

void hardened_private_child_from_private(extended_private_key_t *parent, extended_private_key_t *child, uint hardened_child_number) {

  uint child_number = (1 << 31) + hardened_child_number;
  uchar hmacsha512_result[64] = { 0 };
  uchar hmac_input[37] = {0};
  for(int x=0;x<32;x++){
    hmac_input[x+1] = parent->private_key.key[x];
  }
  hmac_input[33] = child_number >> 24;
  hmac_input[34] = (child_number & 0x00FF0000) >> 16;
  hmac_input[35] = (child_number & 0x0000FF00) >> 8;
  hmac_input[36] = (child_number & 0x000000FF);
  
  hmac_sha512(&parent->chain_code, 32, &hmac_input, 37, &hmacsha512_result);
  
  private_key_t sk;
  sk.compressed = true;
  sk.network = parent->network;
  memcpy(&sk.key, &hmacsha512_result, 32);
  secp256k1_ec_seckey_tweak_add(&sk.key, &parent->private_key.key);
  child->network = parent->network;
  child->depth = parent->depth + 1;
  child->child_number = child_number;
  child->private_key = sk;
  memcpy_offset(&child->chain_code, &hmacsha512_result, 32, 32);
}

typedef struct {
    uchar key[32];
    uchar cc[32];
} keyBuffer;

typedef struct {
    uint path;
} pathBuffer;

/*
void print_wg_size(__constant const char *name) {
    size_t g = get_global_size(0);
    size_t l = get_local_size(0);
    size_t wg = get_num_groups(0);
    printf(""%s(): global=%ld local=%ld groups=%ld\n"", name, g, l, wg);
}
*/

__kernel void bip32_derive_hardened(__global keyBuffer *parent, __global keyBuffer *child, __global pathBuffer *pathBuffer) {

  ulong idx = get_global_id(0);

/*
  //  debug
  if (idx == 0)
  {
    print_wg_size((__constant char*)""bip32_derive_hardened"");
  }
  */

  uint child_number = (1 << 31) | pathBuffer[0].path;
  uchar hmacsha512_result[64] = { 0 };
  uchar hmac_input[37] = {0};
  for(int x=0;x<32;x++){
    hmac_input[x+1] = parent[idx].key[x];
  }
  hmac_input[33] = child_number >> 24;
  hmac_input[34] = (child_number & 0x00FF0000) >> 16;
  hmac_input[35] = (child_number & 0x0000FF00) >> 8;
  hmac_input[36] = (child_number & 0x000000FF);
  
  hmac_sha512(parent[idx].cc, 32, &hmac_input, 37, &hmacsha512_result);
  
  memcpy(child[idx].key, &hmacsha512_result, 32);
  secp256k1_ec_seckey_tweak_add(child[idx].key, parent[idx].key);
  memcpy_offset(child[idx].cc, &hmacsha512_result, 32, 32);
}

__kernel void bip32_derive_normal(__global keyBuffer *parent, __global keyBuffer *child, __global pathBuffer *pathBuffer) {
  ulong idx = get_global_id(0);

/*
  //  debug
  if (idx == 0)
  {
    print_wg_size((__constant char*)""bip32_derive_normal"");
  }
*/
  uchar hmacsha512_result[64] = { 0 };
  //extended_public_key_t pub;
  //public_from_private(parent, &pub);
  uchar pub[32] = { 0 };
  secp256k1_ec_pubkey_create(&pub, parent[idx].key);
  uchar hmac_input[37] = {0};
  //serialized_public_key(&pub, &hmac_input);
  secp256k1_ec_pubkey_serialize(&hmac_input, 33, &pub, SECP256K1_EC_COMPRESSED);
  hmac_input[33] = pathBuffer[0].path >> 24;
  hmac_input[34] = (pathBuffer[0].path & 0x00FF0000) >> 16;
  hmac_input[35] = (pathBuffer[0].path & 0x0000FF00) >> 8;
  hmac_input[36] = (pathBuffer[0].path & 0x000000FF);
  hmac_sha512(parent[idx].cc, 32, &hmac_input, 37, &hmacsha512_result);

  memcpy(child[idx].key, &hmacsha512_result, 32);
  secp256k1_ec_seckey_tweak_add(child[idx].key, parent[idx].key);
  memcpy_offset(child[idx].cc, &hmacsha512_result, 32, 32);
}

        ";

        public static string common_cl = @"
#define uint32_t uint
#define uint64_t ulong
#define uint8_t uchar
#define NULL 0

static void memset(uchar *str, int c, size_t n){
  for(int i=0;i<n;i++){
    str[i] = c;
  }
}

static void memcpy(uchar *dest, uchar *src, size_t n){
  for(int i=0;i<n;i++){
    dest[i] = src[i];
  }
}

static void memcpy_offset(uchar *dest, uchar *src, int offset, uchar bytes_to_copy){
  for(int i=0;i<bytes_to_copy;i++){
    dest[i] = src[offset+i];
  }
}

static void memzero(void *const pnt, const size_t len) {
  volatile unsigned char *volatile pnt_ = (volatile unsigned char *volatile)pnt;
  size_t i = (size_t)0U;

  while (i < len) {
    pnt_[i++] = 0U;
  }
}

static void memczero(void *s, size_t len, int flag) {
    unsigned char *p = (unsigned char *)s;
    volatile int vflag = flag;
    unsigned char mask = -(unsigned char) vflag;
    while (len) {
        *p &= ~mask;
        p++;
        len--;
    }
}

void copy_pad_previous(uchar *pad, uchar *previous, uchar *joined) {
  for(int x=0;x<128;x++){
    joined[x] = pad[x];
  }
  for(int x=0;x<64;x++){
    joined[x+128] = previous[x];
  }
}

void print_byte_array_hex(uchar *arr, int len) {
  for (int i = 0; i < len; i++) {
    printf(""%02x"", arr[i]);
  }
  printf(""\n\n"");
}

void xor_seed_with_round(char *seed, char *round) {
  for(int x=0;x<64;x++){
    seed[x] = seed[x] ^ round[x];
  }
}

void print_seed(uchar *seed){
  printf(""seed: "");
  print_byte_array_hex(seed, 64);
}

void print_raw_address(uchar *address){
  printf(""address: "");
  print_byte_array_hex(address, 25);
}

void print_mnemonic(uchar *mnemonic) {
  printf(""mnemonic: "");
  for(int i=0;i<120;i++){
    printf(""%c"", mnemonic[i]);
  }
  printf(""\n"");
}

void print_byte_array(uchar *arr, int len) {
  printf(""["");
  for(int x=0;x<len;x++){
    printf(""%u"", arr[x]);
    if(x < len-1){
      printf("", "");
    }
  }
  printf(""]\n"");
}
";


    }
}
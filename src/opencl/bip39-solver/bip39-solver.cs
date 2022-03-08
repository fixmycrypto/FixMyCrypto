namespace FixMyCrypto {
    class Bip39_Solver {
        public static string int_to_address_cl = @"

#define inBufferSize <inBufferSize_bytes>
#define outBufferSize <outBufferSize_bytes>
#define saltBufferSize <saltBufferSize_bytes>
#define pwdBufferSize <pwdBufferSize_bytes>
#define hashlength 64

        typedef struct {
            uint length; // in bytes
            uchar buffer[inBufferSize];
        } inbuf;

        typedef struct {
            uchar buffer[outBufferSize];
        } outbuf;

        // Salt buffer, used by pbkdf2 & pbe
        typedef struct {
            uint length; // in bytes
            uchar buffer[saltBufferSize];
        } saltbuf;

        // Password buffer, used by pbkdf2 & pbe
        typedef struct {
            uint length; // in bytes
            uchar buffer[pwdBufferSize];
        } pwdbuf;

        void copy_pad_previous(uchar *pad, uchar *previous, uchar *joined) {
            for(int x=0;x<128;x++){
                joined[x] = pad[x];
            }
            for(int x=0;x<hashlength;x++){
                joined[x+128] = previous[x];
            }
        }

    void xor_seed_with_round(char *seed, char *round) {
        for(int x=0;x<hashlength;x++){
            seed[x] = seed[x] ^ round[x];
        }
    }

        __kernel void pbkdf2(__global inbuf *inbuffer, __global const saltbuf *saltbuffer, __global outbuf *outbuffer,
    __private unsigned int iters, __private unsigned int dkLen_bytes) {

        ulong idx = get_global_id(0);
        uchar ipad_key[128];
        uchar opad_key[128];

        __global uchar *pwd = inbuffer[idx].buffer;
        __global uchar *seed = outbuffer[idx].buffer;
        __global uchar *salt = saltbuffer[0].buffer;
        uint pwdLen = inbuffer[idx].length;
        uint saltLen = saltbuffer[0].length;

        printf(""pwd: %s\n"", pwd);
        printf(""salt: %s\n"", salt);

        int blocks = outBufferSize / hashlength;
        printf(""dkLen_bytes=%d outBufferSize=%d blocks=%d\n"", dkLen_bytes, outBufferSize, blocks);

        for (int block = 1; block <= blocks; block++) {
            for(int x=0;x<128;x++){
                ipad_key[x] = 0x36;
                opad_key[x] = 0x5c;
            }

            for(uint x=0;x<pwdLen && x < 128;x++){
                ipad_key[x] = ipad_key[x] ^ pwd[x];
                opad_key[x] = opad_key[x] ^ pwd[x];
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
            key_previous_concat[x++] = 0;
            key_previous_concat[x++] = 0;
            key_previous_concat[x++] = 0;
            key_previous_concat[x++] = (uchar)block;

            sha512(&key_previous_concat, x, &sha512_result);
            copy_pad_previous(&opad_key, &sha512_result, &key_previous_concat);
            sha512(&key_previous_concat, 192, &sha512_result);
            xor_seed_with_round(seed, &sha512_result);

            for(int x=1;x<iters;x++){
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
        pbkdf2(inbuffer, saltbuffer, outbuffer, 2048, 64);
    }

   __kernel void pbkdf2_4096_96(__global inbuf *inbuffer, __global const saltbuf *saltbuffer, __global outbuf *outbuffer) {
        pbkdf2(inbuffer, saltbuffer, outbuffer, 4096, 96);
    }
        ";
    }
}
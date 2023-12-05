
export interface ConnectDbInput
{
    key: string;
    account: string;
    passWord: string;
    address: string;
    port: number;
    type: number;
    trustCert: boolean;
    trustedConnection: boolean;
    encrypt: boolean;
    saveLocal: boolean;
}


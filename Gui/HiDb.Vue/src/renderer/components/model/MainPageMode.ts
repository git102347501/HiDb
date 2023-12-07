
export interface ConnectDbInput
{
    key: string;
    name: string;
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


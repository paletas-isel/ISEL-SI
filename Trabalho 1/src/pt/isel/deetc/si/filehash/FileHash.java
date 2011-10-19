package pt.isel.deetc.si.filehash;

import pt.isel.deetc.common.clp.CommandLineParser;

import java.io.FileInputStream;
import java.io.FileNotFoundException;
import java.io.IOException;
import java.security.MessageDigest;
import java.security.NoSuchAlgorithmException;
import java.util.Map;

public class FileHash {
    public static void main(String[] args) {
        Map<String, String> params;
        CommandLineParser parser = new CommandLineParser();
        params = parser.parse(args);

        try {
            FileInputStream stream = new FileInputStream(params.get("file"));

            MessageDigest hashAlgorithm = MessageDigest.getInstance(params.get("alg"));

            byte[] read = new byte[4 * 1024];
            int nrBytesRead;
            while((nrBytesRead = stream.read(read)) != -1){
                hashAlgorithm.update(read, 0, nrBytesRead);
            }

            byte[] res = hashAlgorithm.digest();

            System.out.printf("%X", res[0]);
            for(int x = 1; x < res.length; ++x) {
                System.out.printf(" %X", res[x]);
            }
        } catch (FileNotFoundException e) {
            System.err.println(String.format("File not found! Searched for: %s", params.get("file")));
        } catch (NoSuchAlgorithmException e) {
            System.err.println(String.format("Requested algorithm (%s) isn't valid.", params.get("alg")));
        } catch (IOException e) {
            System.err.println(e.getMessage());
        }
    }

}

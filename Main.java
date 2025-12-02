public class Main {
    public static void main(String[] args){
        // Zmieniono i<=10 na i<=5
        for (int i=1;i<=5;i++){ 
            // Zmieniono j<=10 na j<=5
            for (int j=1;j<=5;++j){ 
                System.out.printf("%5s" , i*j);
            }
            System.out.println();
        }
    }
}

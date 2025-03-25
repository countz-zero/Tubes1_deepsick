# Robocode Tank Royale Bots

4 Bot untuk permainan Robocode. Masing-masing bot mengimplementasikan algoritma greedy dengan strategi yang berbeda-beda.

# Author

Dzubyan Ilman Ramadhan (10122010)

Rendi (10123083)

# Requirement

Operating System : Windows, macOS, atau Linux

.NET versi terbaru

Java versi terbaru

Aplikasi Robocode Tank Royale

# Rincian Algoritma

* Erlang
Bot ini greedy terhadap energi. Berusaha untuk menghindari serangan lawan di tengah-tengah arena dan menggunakan energi seefisien mungkin, namun belum cukup baik dalam menghindari serangan.
* Wukong
Bot ini greedy terhadap damage. Berusaha untuk memperoleh skor dengan damage sebesar mungkin terhadap bot lain, namun belum cukup akurat.
* Tembok

  Bot ini greedy terhadap poin dari survival. Bot ini akan bergerak memutar di sekitar pinggir arena pertempuran dan menghindari bot lainnya apabila tertabrak dengan cara kabur ke arah berlawanan dari bot lainnya.
  
* Wintermute

  Bot ini greedy terhadap poin dari menembak bot lain dan posisi bot lainnya. Bot ini akan memindai untuk mencari bot lain pada jangkauan 120 derajat lalu berputar apabila tidak menemukan bot lain. Bot ini akan mencari bot terdekat yang ditangkap radar, lalu akan menargetkan untuk mendekat dan menembak berkali-kali sampai mati atau terdapat bot lain yang lebih dekat yang dideteksinya.

# Cara menggunakan 
1. Download file ZIP dari github
2. Download Robocode Tank Royale di [laman github ini](https://github.com/robocode-dev/tank-royale)
3. Setelah ter-download, buka aplikasi Robocode Tank Royale, dan buka menu Config, lalu Bot Root Directory
4. Pilih tombol Add lalu tambahkan **folder yang berisi folder-folder bot** (alternative-bots & main-bots)
5. Buka menu Battle, lalu Start Battle
6. Semua file bot akan muncul di box **Bot Directories**, pilih bot yang ingin dimainkan lalu tekan tombol Boot
7. Apabila nama bot telah muncul di box Joined Bots, tambahkan bot yang ingin dimainkan hingga muncul di box Selected Bots
8. Tekan tombol Start Battle

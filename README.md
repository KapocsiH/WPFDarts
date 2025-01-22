# WPF Darts
Ez a projekt egy WPF-ben megvalósított darts játék, amely három különböző játékmódot tartalmaz: **501**, **Around the Clock** és **Shanghai**. A játék lehetőséget biztosít a játékosok számára, hogy különböző szabályrendszerek alapján mérkék össze tudásukat.

## Működése
- Az XAML felületen egy dartstáblára az egérrel lehet célozni
- A célzást nehezíti egy "kurzor rázkódás"
- Játékmód választási lehetőség

## A játékmódok

### 501
- A játékosok 501 pontról indulnak
- Az nyer, aki először éri el pontosan a 0 pontot
- Az utolsó dobásnak egy dupla szektornak kell lennie

### Around the Clock
- A játékosok az 1-es szektorról indulnak
- Szektoronként növekvő sorrendben kell haladni
- Az nyer, aki először eléri a 20-as szektort

### Sanghai
- A játékosok az 1-es szektorról indulnak
- 7 kör van
- Szektoronként növekvő sorrendben kell haladni
- Minden szektorban a lehető legtöbb pontot kell elérni
- Az nyer, aki a végére a legtöbb pontot szerzi
- Ha a játékos dob egy Shanghait (ugyanazon szektorban egy szimpla, egy dupla és egy tripla szektor), azonnal megyneri a játékot

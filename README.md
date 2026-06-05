# Vonder Games - Game Developer Assignment

ฉากแบบ 2D Interactive ที่สร้างด้วย Unity ประกอบด้วยการควบคุมตัวละคร, การโต้ตอบกับ NPC, ระบบบทสนทนา (Dialogue), การเก็บไอเทมตามเป้าหมาย และลำดับเหตุการณ์ที่ขับเคลื่อนด้วย Timeline

ลองเล่นผ่าน SampleScene ได้เลย

ใน GameMaanger ทำ state machine ง่ายโดยใช้ Enum เป็นตัวบอก Step แล้วมา ConfigureSteps แต่ละ step เองว่าจะให้ทำอะไรยังไงบ้าง ส่วนข้อมูลก็สร้าง class มาเก็บไว้โดยใช้ enum step มาเลียกใช้ข้อมูลไป มีข้อมูล Dialogue และ TimeLine ทำตามขั้นตอนไปเรื่อยๆจนจบ

playerController คุมด้วย InputSystem และคุม animation กับเสียงไปในตัวเลย NPC ก็คล้ายกันแต่ไม่ได้คุมเอง 

Dialogue มี 3 ส่วน Dialogue DialoguePanel คุม Dialogue ทั้งหมดการเปิดปิดส่งข้มูลไปแสดงเล่นเสียง DialogueBox คุมการแสดงผลของ dialogue typing character ที่แสดงผล แล้วก็ ScriptableObject DialogueData เอาไว้เซ็ดข้อมูลที่จะเอามาแสดงใน Dialogue 

ระบบ InterAction สร้างเป็นตัวกล้างขึ้นมาในการให้ player ไป Interaction ด้วย โดยตัวที่สามารถส่ง event ไปเผื่อให้ player มา Trigger ให้ event ทำงาน ใช้ตัวนี้ในการ Trigger dialogue แล้วก็เก็บ item

ใน TimeLine หลักๆ ใช้การเปิดปิดกล้อง CinemachineCamera ในการแผนไปมา Timeline Signals ใช้ Disable playerController ในระหว่างที่ cutscene ทำงานอยู่ ใช้ Trigger Animation warp ของ NPC

Video มีใน Floder Recordings

## Assets
BG : https://free-game-assets.itch.io/nature-landscapes-free-pixel-art
UI Dialogue : https://assetstore.unity.com/packages/2d/gui/fantasy-wooden-gui-free-103811
tile and pop : https://assetstore.unity.com/packages/2d/environments/pixel-art-platformer-village-props-166114
sound : https://freetouse.com/music
vfx : https://assetstore.unity.com/packages/vfx/particles/cartoon-fx-remaster-free-109565
DoTween : https://dotween.demigiant.com/

## Time Breakdown

(2026.06.05)
- 10.00-11.00 ตั้งค่าโปรเจกต์: สร้างโปรเจกต์ Unity, จัดโครงสร้างโฟลเดอร์, import แพ็กเกจ Input System, Cinemachine, Timeline และ DoTween
- 11.00-12.00 ฉากและ Asset: นำเข้าสไปรต์/ไฟล์เสียง และจัดวางแผนที่ (tilemap, สภาพแวดล้อม, กล้อง)
- 13.00-14.30 การควบคุมตัวละคร: ทำระบบเดินซ้าย/ขวาแบบ 2D และกระโดดด้วย Input System ใหม่, ตรวจจับพื้น (ground check) และสลับแอนิเมชัน idle/walk/jump
- 14.30-15.30 ขัดเกลาตัวละคร: เพิ่มเสียงฝีเท้าและเสียงกระโดด, พลิกสไปรต์ตามทิศทางที่หันหน้า
- 15.30-16.30 UI และ Tweening: สร้างเลเยอร์ UI, integrate DoTween และทำเอฟเฟกต์ป้ายข้อความเต้นด้วย `ScaleLoop`
- 16.30-18.00 ระบบบทสนทนา: สร้าง `DialogueData`, `DialogueBox` และ `DialoguePanel` พร้อมแอนิเมชันเลื่อนเข้า/ออก และเสียงต่อบรรทัด

(2026.06.06)
- 10.00-11.30 ระบบ InterAction: เพิ่ม `InterActionZone` ตรวจจับระยะ พร้อมปุ่ม E สำหรับโต้ตอบกับ NPC และไอเทม
- 11.30-12.30 พฤติกรรม NPC: ทำ `NPCController` ให้เดินสุ่ม (สุ่มเวลาการเดิน/หยุด) และหันหน้าตามการตรวจจับผู้เล่น
- 13.00-14.30 ลำดับเกม: สร้าง state machine แบบ step ใน `GameManager`, dialogue step และการจัดการเป้าหมายใน `ObjectivePickupItem`
- 14.30-15.30 Timeline และเสียง: ควบคุมลำดับ Timeline ผ่าน `PlayableDirector` จาก GameManager และเชื่อมเสียงฉาก/คัตซีน
- 15.30-16.30 ทดสอบและเก็บงาน: เล่นทดสอบทั้งฟลว์, แก้บั๊ก และจัดระเบียบฉากกับสคริปต์

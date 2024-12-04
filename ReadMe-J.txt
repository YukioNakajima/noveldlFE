2024/12/04　更新

１．概要
Naro2mobi用のダウンローダを使用した小説ダウンロードソフト
noveldlFrontEndのiniファイルに登録されているリストファイルに小説情報を登録できる
名称：noveldlFE
GitHub:https://github.com/YukioNakajima/noveldlFE.git

２．開発環境
　Microsoft Visual Studio Community 2019、C#

２．動作環境
　Windows7,8,10,11のはず

４．動作に必要なもの
各種Naro2mobi用のダウンローダ、以下のもので動作確認（2024/12/04）しています
exeと同じフォルダに動作するよう配置してください。
・na6dl32
・kakuyomudl

・noveldlFrontEndのiniファイル

以降はnoveldlFrontEndのiniファイルに登録されていて間接的に必要なもの

・ダウンロードする小説を指定するリストファイル
テキストファイル、UTF-8
>>
Type:小説家になろう
DL Folder:C:\小説\narou_TXT_DL
Novel Folder:C:\小説\なろう

#毎日
小説名１.txt
https://novel18.syosetu.com/nXXXXAA/
小説２フォルダ\小説名２.txt
https://novel18.syosetu.com/nXXXXAA/

#毎週
小説３フォルダ\小説名３.txt
https://novel18.syosetu.com/nXXXXAA/

#毎月
小説４フォルダ\小説名４.txt
https://novel18.syosetu.com/nXXXXAA/

#完結

#消滅

##
<<

※"DL Folder:"、"Novel Folder:"、"#毎日"、"#毎週"、"#毎月"、その後の行頭が"#"の行の位置関係を変えないでください。

上記内容だと、以下のようなフォルダ構成となります

C:\小説\なろう
　＋小説名１.txt
　＋小説２フォルダ
　｜　+\小説名２.txt
　＋小説３フォルダ
　｜　+\小説名３.txt
　＋小説４フォルダ
　｜　+\小説名４.txt

"#毎日"の小説は最後の更新から１２時間経過するとダウンロード可能です。
"#毎日""#毎週"の小説は最後の"#毎週"更新後の次週の月曜以降にダウンロード可能です。
"#毎日""#毎週""#毎月"の小説は最後の"#毎月"更新後の次月の１日以降にダウンロード可能です。
"#完結"には完結した小説の情報をここに移動するようにする予定
"#消滅"にはURLに小説がなくなった場合に小説の情報をここに移動するようにする予定
"##"はファイル末尾を示す。これの代わりに行頭が#の文字列で置き換えられる。


５．インストール
・適当なフォルダにexeファイルを置きます。
・同じフォルダにna6dl32.exeと必要なファイルを置きます。
・ダウンロードしたい小説のリストファイルを作成します。
　上記"ダウンロードする小説を指定するリストファイル"を参照してください。

６．使用方法
・noveldlFEを実行してください。
・ダウンロードしたい小説サイト用のリストファイルを左下のコンボボックスから選択します。
　対応するリストファイルがない場合はnoveldlFrintEndの説明をみて作成してください。
　ダウンロードに使用するダウンローダを判定するのに使用します。
・ダウンロードしたい小説のURLを中段のテキストボックスにコピー＆ペーストしてください。
この先は小説情報をリストファイルに登録するかしないかで変わります。

登録しない場合
・リストファイルに登録しない場合は下段の右のテキストボックスを空白にしてください。
　この時、noveldlFE.exeのあるフォルダに小説ファイルが作成されます
・ダウンロードボタンのクリックでダウンロード開始します。

登録する場合
・リストに登録する場合は、下段の右のテキストボックスにリストファイルのベースからの相対パスを指定します。
　（例：作者名\格納フォルダ名\小説名.txt）
・ダウンロードボタンのクリックでダウンロード開始します。
　リストファイルの「#毎週」の最後に小説情報が登録されます

※各フォルダに"小説名Info.txt"というファイルが作成されます。
内容は以下のように最初の行は、更新日時と、最後の章番号です。
2行目以降は挿絵のURLです
>>
2024/10/20 10:47:49, 111
https://xxxxxxx
https://xxxxxxx
<<
このファイルから、新しく追加された章を判断して、続きをダウンロード、小説ファイルはマージします。
マージ時に先頭から"［＃中見出し］”の前の行までを無視します。
この時、na6dlFrontEnd.exeと同じフォルダに"tmp.txt"として追加章のファイルをが作成されます。
※ソフト実行後、exeと同じフォルダにINIファイルが作成されます。
リスト内容と、時間更新可能な日時が保存されています。
INIファイルの例をいかに記載します
>>
[ListItems]
Count=2
Item1=F:\Books\na6dl32\_なろう_URLs.txt
Item2=F:\Books\na6dl32\_NN_URLs.txt

[NextDownLoad]
毎月=2024/11/01 0:00:00
毎週=2024/10/21 00:00:00
毎日=2024/10/21 05:52:21
実行日時=2024/10/20 17:52:21

[DownloadAfterOperation]
//初回ダウンロード後処理
Novel1st=""C:\Program Files (x86)\sakura\sakura.exe" "-M=F:\Books\na6dl32\na6dlFE\na6dlFrontEnd0\na6dlFrontEnd\bin\Debug\na6dl用変換-自動.vbs" "%F""
//2回目以降の差分ダウンロード後の差分ファイルに対しての処理
Novel1Later=""C:\Program Files (x86)\sakura\sakura.exe" "-M=F:\Books\na6dl32\na6dlFE\na6dlFrontEnd0\na6dlFrontEnd\bin\Debug\na6dl用章追加変換-自動.vbs" "%F""

[小説家になろう]
Downloader="na6dl.exe"
URLTop="https://ncode.syosetu.com/n,https://novel18.syosetu.com/n"

[NN]
Downloader="na6dl.exe"
URLTop="https://ncode.syosetu.com/n,https://novel18.syosetu.com/n"

[カクヨム]
Downloader="kakuyomudl.exe"
URLTop="https://kakuyomu.jp/works/"
<<
[ListItems]はダウンロードリストファイルのリストを記憶します。
[NextDownLoad]は次回にダウンロード可能な日時を記憶します。
[DownloadAfterOperation]は一つの小説をダウンロードした時に実行する外部プログラムを記載します。
Novel1st：初めて小説をダウンロードした後で実行します。なければ実行しません。
NovelLator：2回目以降に前回との差分をダウンロードした後で実行します。差分ファイルに対して実行します。なければ実行しません。
全体を引用符(")で囲み、引数１つ１つを引用符(")で囲んでください。
「%F」はダウンロードファイル名に置換されます。

「[小説家になろう]」はリストファイルの「Type:小説家になろう」に対応しています。
「Downloader=」はこのファイルの小説をダウンロードするのに使用するNaro2mobi用のダウンローダのexeファイルを指定します
「URLTop=」は小説URLの先頭が妥当かどうかチェックする文字列です。","で区切って複数登録可能です。



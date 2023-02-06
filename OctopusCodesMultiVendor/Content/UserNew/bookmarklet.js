var whi = whi || {};
whi.website = "http://weheartit.com/";

whi.i18n = {};
whi.i18n.locale = 'en';
whi.i18n._messages = {};

whi.i18n.t = function (key, partials) {
    var currentMessage = whi.i18n._extract(whi.i18n._localeMessages(whi.i18n.locale), key);
    currentMessage = whi.i18n._applyPartials(currentMessage, partials);

   var defaultMessage = whi.i18n._extract(whi.i18n._localeMessages('en'), key);
    defaultMessage = whi.i18n._applyPartials(defaultMessage, partials);

    return currentMessage || defaultMessage || key;
};

whi.i18n._extract = function (rootObject, key) {
    if (!rootObject) { return; }

    var parts = key.split('.');
    var currentKey = parts.shift();
    return parts.length > 0 ? whi.i18n._extract(rootObject[currentKey], parts.join('.')) : rootObject[currentKey];
};

whi.i18n._applyPartials = function (text, partials) {
    if (!text || typeof text !== "string") return text;

    var keys = text.match(/%{(.+)}/) || [];
    keys.slice(1).forEach(function (partialKey) {
      partialValue = partials[partialKey] || partialKey;
      text = text.replace('%{' + partialKey + '}', partialValue);
    });
    return text;
};

whi.i18n._localeMessages = function (locale) {
    return whi.i18n._messages[locale];
};

// Do not delete me, I'm a Safari hack

whi.i18n._messages = {
  "ar": {
    "bookmarklet": {
      "no_set": "-لا مجموعة-",
      "new_set": "-مجموعة جديدة-",
      "new_set_name": "إسم مجموعة جديد",
      "must_log_in_to_save_to_set": "يجب أن تقوم بتسجيل الدخول إلى We Heart It لحفظها في مجموعة.",
      "no_image_we_can_heart": "لم نتمكن من العثور على أي صور يمكن أن تحبها على هذه الصفحة.",
      "why_is_that_q": "لم ذلك؟",
      "you_can_only_heart_on_websites": "يمكنك فقط أن تحب صور من المواقع،",
      "not_directly": "ليس الصورة مباشرة في المتصفح.",
      "go_back_to_site_and_heart": "الرجاء عد إلى الموقع وأحب الصور من هناك.",
      "save_image_to_my_collection": "حفظ الصورة في مجموعتي",
      "download_new_whi_button": "تنزيل زر القلب الجديد",
      "tap_the_image": "إضغط على الصورة التي تريد أن تحبها.",
      "move_your_mouse_over": "حرك الفأرة فوق الصورة التي تريد أن تحبها.",
      "alert_no_heart": "إن هذا الموقع لا يسمح ل We Heart It بحب الأشياء. الرجاء مراسلة صاحب الموقع إذا كانت لديك أية أسئلة. شكراً لزيارتك!",
      "alert_no_pin": "                     إن هذا الموقع لا يسمح بحفظ مواقع مثل We Heart It.​ الرجاء مراسلة صاحب الموقع إذا كانت لديك أية أسئلة.​ شكراً لزيارتك!                 ",
      "alert_we_heart_it": "أنت تحاول استخدام زر القلب على weheartit.com، لكن على موقعنا يمكنك فقط النقر على رمز القلب فوق الصورة.",
      "need_help_q": "هل تحتاج إلى بعض المساعدة في ذلك؟",
      "alert_private": "أنت تحاول الحصول على صورة من صفحة يمكنك وحدك الدخول إليها. الرجاء إفتح الرابط الثابت في نافذة أخرى وأحب من هناك.",
      "alert_public": "الصور على %{website} هي بشكل عام خاصة. ",
      "alert_whi_posting_public": "عند نشر صورة على We Heart It تصبح متاحة للجميع وقد يظهر إسم الشخص الذي نشرها على %{website} على الموقع.",
      "alert_confirm": "قرأت وفهمت",
      "alert_google_images": "لا يمكنك أن تحب صور في بحث جوجل. الرجاء إفتح رابط الصورة وأحبها من هناك.",
      "alert_blocked": "تم حظر هذا الموقع من قبل weheartit.com. إذا كنت ترغب في تقديم طلب لإعادة النظر، الرجاء القيام بذلك %{appeal_link}.",
      "appeal_here": "هنا",
      "blocked": "تم حظرها",
      "before_you_heart": "قبل أن تحب...",
      "hey": "مرحباً!",
      "new_release_notice": "لقد أطلقنا للتو زر القلب الجديد الخاص بنا. لقد أصبح الآن مندمج بشكل أفضل مع المتصفح الخاص بك كملحق كامل، لذا فهو فعال أكثر ويعمل مع كافة المواقع التي تقوم بزيارتها وتحبها.",
      "please_upgrade": "الرجاء قم بالتحديث وتابع حبك للأشياء!",
      "learn_more_and_upgrade": "لمعرفة المزيد والتحديث",
      "new_button_available": "زر القلب الجديد متوفر!",
      "close_button": "غلق",
      "title": "عنوان",
      "add_title": "أضف عنوان إلى هذه الصورة",
      "tags": "إشارات",
      "separate_with_commas": "إفصل بفواصل",
      "close": "غلق"
    },
    "extension": {
      "save_image_to_my_collection": "حفظ الصورة في مجموعتي",
      "form": {
        "title": "عنوان",
        "add_title_to_this_image": "أضف عنواناً إلى هذه الصرز",
        "add_tags": "أضف إشارات",
        "tags_describe_image": "الإشارات هي كلمات تصف الصورة",
        "tags_example": "مثلاً: تصوير، أسود وأبيض، فتاة"
      },
      "sets": {
        "no_set": "-لا مجموعة-",
        "new_set": "-مجموعة جديدة-",
        "save_to_set": "حفظ في مجموعة",
        "new_set_name": "إسم مجموعة جديد",
        "must_log_in_to_save_to_set": "يجب أن تقوم بتسجيل الدخول إلى We Heart It لحفظها في مجموعة."
      },
      "drop": {
        "drag_the_image_here": "إسحب الصورة إلى هنا لحفظها في مجموعتك"
      }
    },
    "extension_specific": {
      "chrome": {
        "extension_name": "We Heart It",
        "extension_description": "امتداد We Heart It جوجل كروم الرسمي",
        "action_default_title": "أحب صورة على هذه الصفحة!"
      },
      "safari": {
        "name": "We Heart It",
        "author": "شركة WHI",
        "description": "إحفظ صورك المفضلة على حساب weheartit الخاص بك."
      },
      "firefox": {
        "name": "زر القلب",
        "description": "زر القلب من We Heart It (شركة WHI)",
        "creator": "We Heart It (شركة WHI)"
      }
    }
  },
  "de": {
    "bookmarklet": {
      "no_set": "- Keine Sammlung - ",
      "new_set": "- Neue Sammlung -",
      "new_set_name": "Name der neuen Sammlung",
      "must_log_in_to_save_to_set": "Du musst bei We Heart It angemeldet sein, um etwas in einer Sammlung speichern zu können.",
      "no_image_we_can_heart": "Wir konnten kein Bild auf dieser Seite finden, das du herzen könntest.",
      "why_is_that_q": "Warum ist das so?",
      "you_can_only_heart_on_websites": "Du kannst nur Bilder von Websites herzen,",
      "not_directly": "nicht das Bild direkt im Browser.",
      "go_back_to_site_and_heart": "Gehe zurück auf die Website und herze von dort aus.",
      "save_image_to_my_collection": "Bild in meiner Sammlung speichern",
      "download_new_whi_button": "Den neuen Herz-Button herunterladen",
      "tap_the_image": "Tippe auf das Bild, das du herzen möchtest.",
      "move_your_mouse_over": "Bewege den Mauszeiger über das Bild, das du herzen möchtest.",
      "alert_no_heart": "Diese Website gestattet das herzen auf We Heart It nicht. Bei Fragen wende dich bitte an den Eigentümer der Website. Vielen Dank für deinen Besuch.",
      "alert_no_pin": "Diese Website gestattet das Setzen von Lesezeichen auf Websites wie We Heart It nicht. Bei Fragen wende dich bitte an den Eigentümer der Website. Vielen Dank für deinen Besuch.",
      "alert_we_heart_it": "Du versuchst, den Heart It-Button auf weheartit.com zu verwenden, aber auf deiner Website kannst du einfach nur auf das Herzsymbol auf dem Bild klicken.",
      "need_help_q": "brauchst du Hilfe damit?",
      "alert_private": "Du versuchst Bilder von einer Seite abzurufen, zu der nur du Zugang hast. Öffne den Permalink in einem anderen Fenster und herze von dort aus.",
      "alert_public": "Bilder auf %{website} sind normalerweise privat.",
      "alert_whi_posting_public": "Wenn du ein Bild auf We Heart It postest, wird es öffentlich, und es kann sein, dass der Name der Person, die es auf %{website} gepostet hat, auf der Website erscheint.",
      "alert_confirm": "Dies habe ich gelesen und verstanden",
      "alert_google_images": "Du kannst Bilder nicht in der Google-​Suche herzen.​ Öffne bitte den Link zum Bild und herze dort.​",
      "alert_blocked": "Die Website wurde von weheartit.com gesperrt. Wenn du Einspruch einlegen möchtest, tu dies bitte %{appeal_link}",
      "appeal_here": "hier tun",
      "blocked": "Gesperrt",
      "before_you_heart": "Bevor du loslegst, ...",
      "hey": "Hallo!",
      "new_release_notice": "Wir haben gerade unseren neuen Herz-Button veröffentlicht. Als volle Erweiterung ist er besser mit deinem Browser integriert, er ist also zuverlässiger und funktioniert auf allen Websites, die du besuchst und die dir gefallen.",
      "please_upgrade": "Bitte aktualisiere ihn und mach' weiter so!",
      "learn_more_and_upgrade": "Weitere Informationen und Upgrade",
      "new_button_available": "Der neue Herz-Button ist da!",
      "close_button": "Schließen",
      "title": "Titel",
      "add_title": "Gib diesem Bild einen Titel",
      "tags": "Tags",
      "separate_with_commas": "durch Komma voneinander trennen",
      "close": "Schließen",
      "click_to_heart": "zum Herzen auf die Bilder klicken"
    },
    "extension": {
      "save_image_to_my_collection": "Bild in meiner Sammlung speichern",
      "form": {
        "title": "Titel",
        "add_title_to_this_image": "Diesem Bild einen Titel hinzufügen",
        "add_tags": "Tags hinzufügen",
        "tags_describe_image": "Tags sind Wörter, die das Bild beschreiben.",
        "tags_example": "Z. B. Fotografie, schwarzweiss, Mädchen"
      },
      "sets": {
        "no_set": "- Keine Sammlung - ",
        "new_set": "- Neue Sammlung -",
        "save_to_set": "In Sammlung speichern",
        "new_set_name": "Name der neuen Sammlung",
        "must_log_in_to_save_to_set": "Du musst bei We Heart It angemeldet sein, um etwas in einer Sammlung speichern zu können."
      },
      "drop": {
        "drag_the_image_here": "Ziehe das Bild hier, um es in deiner Sammlung zu speichern."
      }
    },
    "extension_specific": {
      "chrome": {
        "extension_name": "We Heart It",
        "extension_description": "Offizielle Google Chrome-Erweiterung für We Heart It",
        "action_default_title": "Herze ein Bild auf dieser Seite!"
      },
      "safari": {
        "name": "We Heart It",
        "author": "Super Basic, LLC.",
        "description": "Speichere deine Lieblingsbilder auf deinem We Heart It-Konto."
      },
      "firefox": {
        "name": "Herz-Button",
        "description": "Herz-Button von We Heart It (Super Basic, LLC.)",
        "creator": "We Heart It (Super Basic, LLC.)"
      }
    }
  },
  "el": {
    "bookmarklet": {
      "no_set": "-Καμία ομάδα-",
      "new_set": "-Νέα ομάδα-",
      "new_set_name": "Νέο όνομα ομάδας",
      "must_log_in_to_save_to_set": "Πρέπει να είστε συνδεδεμένοι στο We Heart It για να αποθηκεύσετε σε ομάδα.",
      "no_image_we_can_heart": "Δε μπορέσαμε να βρούμε εικόνες τις οποίες μπορείτε να βάλετε σε καρδιά σε αυτή τη σελίδα.",
      "why_is_that_q": "Γιατί συμβαίνει αυτό;",
      "you_can_only_heart_on_websites": "Μπορείτε να βάλετε σε καρδιά μόνο εικόνες από ιστοσελίδες,",
      "not_directly": "όχι την εικόνα απευθείας στον περιηγητή σας.",
      "go_back_to_site_and_heart": "Παρακαλούμε πηγαίνετε πίσω στην ιστοσελίδα και βάλτε σε καρδιά από εκεί.",
      "save_image_to_my_collection": "Αποθήκευση εικόνας στη συλλογή μου",
      "download_new_whi_button": "Κατεβάστε το νέο Κουμπί Καρδιά",
      "tap_the_image": "Χτυπήστε πάνω στην εικόνα που θέλετε να βάλετε σε καρδιά.",
      "move_your_mouse_over": "Μετακινήστε το ποντίκι σας πάνω από την εικόνα που θέλετε να βάλετε σε καρδιά.",
      "alert_no_heart": "Αυτή η ιστοσελίδα δεν επιτρέπει να βάζετε σε καρδιά το We Heart It. Παρακαλούμε επικοινωνήστε με τον ιδιοκτήτη για τυχόν απορίες. Σας ευχαριστούμε που μας επισκεφτήκατε!",
      "alert_no_pin": "Αυτή η ιστοσελίδα δεν επιτρέπει να βάζετε στους σελιδοδείκτες ιστοσελίδες όπως το We Heart It. παρακαλούμε επικοινωνήστε με τον ιδιοκτήτη για τυχόν απορίες. Σας ευχαριστούμε που μας επισκεφτήκατε!",
      "alert_we_heart_it": "Προσπαθείτε να χρησιμοποιήσετε το Κουμπί Καρδιά στο weheartit.com, αλλά στην ιστοσελίδα μας μπορείτε απλώς να κάνετε κλικ στο εικονίδιο με τη καρδιά πάνω από την εικόνα.",
      "need_help_q": "χρειάζεστε βοήθεια με αυτό;",
      "alert_private": "Προσπαθείτε να πάρετε εικόνες από μία σελίδα στην οποία μόνο εσείς έχετε πρόσβαση. Παρακαλούμε ανοίξτε το μόνιμο σύνδεσμο σε ένα άλλο παράθυρο και βάλτε τη σε καρδιά από εκεί.",
      "alert_public": "Οι εικόνες στο %{website} είναι συνήθως ιδιωτικές.",
      "alert_whi_posting_public": "Η ανάρτηση του στο We Heart It θα κάνει την εικόνα δημόσια και το όνομα αυτού που την ανάρτησε στο %{website} μπορεί να εμφανιστεί στην ιστοσελίδα.",
      "alert_confirm": "Έχω διαβάσει και κατανοήσει",
      "alert_google_images": "Δε μπορείτε να βάλετε καρδιές σε εικόνες στην Αναζήτηση Google. Παρακαλούμε ανοίξτε το σύνδεσμο της εικόνας και βάλτε καρδιά από εκεί.",
      "alert_blocked": "Αυτή η σελίδα έχει αποκλειστεί από το weheartit.com. Αν θέλετε να κάνετε έφεση, παρακαλούμε κάντε την %{appeal_link}.",
      "appeal_here": "εδώ",
      "blocked": "Αποκλείστηκε",
      "before_you_heart": "Πριν βάλετε σε καρδιά...",
      "hey": "Ακούστε!",
      "new_release_notice": "Μόλις κυκλοφόρησε το καινούριο μας Κουμπί Καρδιά. Είναι καλύτερα ενσωματωμένο με τον περιηγητή σας ως πλήρες πρόσθετο για αυτό είναι πιο αξιόπιστο και λειτουργεί με όλες τις ιστοσελίδες που επισκέπτεστε και αγαπάτε.",
      "please_upgrade": "Παρακαλούμε αναβαθμίστε και συνεχίστε να βάζετε καρδιές!",
      "learn_more_and_upgrade": "Μάθετε περισσότερα και αναβαθμίστε",
      "new_button_available": "Νέο Κουμπί Καρδιά διαθέσιμο!",
      "close_button": "κλείσιμο",
      "title": "τίτλος",
      "add_title": "προσθήκη τίτλου στην εικόνα",
      "tags": "ετικέτες",
      "separate_with_commas": "χωρίστε με κόμματα",
      "close": "κλείσιμο",
      "click_to_heart": "κάντε κλικ στις εικόνες για να τις βάλετε σε καρδιά"
    },
    "extension": {
      "save_image_to_my_collection": "Αποθήκευση εικόνας στη συλλογή μου",
      "form": {
        "title": "Τίτλος",
        "add_title_to_this_image": "Προσθήκη τίτλου στην εικόνα",
        "add_tags": "Προσθέστε ετικέτες",
        "tags_describe_image": "Οι ετικέτες είναι λέξεις που περιγράφουν την εικόνα",
        "tags_example": "Π.χ.: Φωτογραφία, Ασπρόμαυρο, Κορίτσι"
      },
      "sets": {
        "no_set": "-Καμία ομάδα-",
        "new_set": "-Νέα ομάδα-",
        "save_to_set": "Αποθήκευση σε ομάδα",
        "new_set_name": "Νέο όνομα ομάδας",
        "must_log_in_to_save_to_set": "Πρέπει να είστε συνδεδεμένοι στο We Heart It για να αποθηκεύσετε σε ομάδα."
      },
      "drop": {
        "drag_the_image_here": "Σύρετε την εικόνα εδώ για να την αποθηκεύσετε στη συλλογή σας"
      }
    },
    "extension_specific": {
      "chrome": {
        "extension_name": "We Heart It",
        "extension_description": "Επίσημο We Heart It πρόσθετο για Google Chrome",
        "action_default_title": "Βάλτε σε καρδιά μια εικόνα σε αυτή τη σελίδα!"
      },
      "safari": {
        "name": "We Heart It",
        "author": "Super Basic, LLC.",
        "description": "Αποθηκεύστε τις αγαπημένες σας εικόνες στο weheartit λογαριασμό σας."
      },
      "firefox": {
        "name": "Κουμπί Καρδιά",
        "description": "Κουμπί Καρδιά από We Heart It (Super Basic, LLC.)",
        "creator": "We Heart It (Super Basic, LLC.)"
      }
    }
  },
  "en": {
    "bookmarklet": {
      "no_set": "-No set-",
      "new_set": "-New set-",
      "new_set_name": "New set name",
      "must_log_in_to_save_to_set": "You must be logged in on We Heart It to save to a set.",
      "no_image_we_can_heart": "We couldn't find any images that you can heart on this page.",
      "why_is_that_q": "Why is that?",
      "you_can_only_heart_on_websites": "You can only heart images from websites,",
      "not_directly": "not the image directly in the browser.",
      "go_back_to_site_and_heart": "Please go back to the site and heart from there.",
      "save_image_to_my_collection": "Save image to my collection",
      "download_new_whi_button": "Download the new Heart Button",
      "tap_the_image": "Tap on the image you want to heart.",
      "move_your_mouse_over": "Move your mouse over the image you want to heart.",
      "alert_no_heart": "This site doesn't allow hearting to We Heart It. Please contact the owner with any questions. Thanks for visiting!",
      "alert_no_pin": "This site doesn’t allow bookmarking to sites like We Heart It. Please contact the owner with any questions. Thanks for visiting!",
      "alert_we_heart_it": "You're trying to use the Heart Button on weheartit.com, but on our site you can just click on the heart symbol over the image.",
      "need_help_q": "need some help with that?",
      "alert_private": "You're trying to get images from a page that only you have access to. Please open the permalink in another window and heart from there.",
      "alert_public": "Images on %{website} are usually private.",
      "alert_whi_posting_public": "Posting it on We Heart It will make the image public and the name of who posted it on %{website} may appear on the site.",
      "alert_confirm": "I've read and understood",
      "alert_google_images": "You can't heart images on Google Search. Please open the image link and heart from there.",
      "alert_blocked": "This site has been blocked by weheartit.com. If you would like to appeal, please do it %{appeal_link}.",
      "error": "Error!",
      "alert_server_error": "We Heart It server is unavailable at the moment. Please try again later.",
      "appeal_here": "here",
      "blocked": "Blocked",
      "before_you_heart": "Before you heart...",
      "hey": "Hey!",
      "new_release_notice": "We've just released our new Heart Button. It's better integrated with your browser as a full extension so it's more reliable and works with all the sites you visit and love.",
      "please_upgrade": "Please upgrade and keep hearting!",
      "learn_more_and_upgrade": "Learn more and upgrade",
      "new_button_available": "New Heart Button available!",
      "close_button": "close",
      "title": "title",
      "add_title": "add a title to this image",
      "tags": "tags",
      "separate_with_commas": "separate with commas",
      "close": "Close",
      "click_to_heart": "click images to heart"
    },
    "extension": {
      "save_image_to_my_collection": "Save image to my collection",
      "form": {
        "title": "Title",
        "add_title_to_this_image": "Add title to this image",
        "add_tags": "Add tags",
        "tags_describe_image": "Tags are words that describe the image",
        "tags_example": "E.g.: Photography, Black and White, Girl"
      },
      "sets": {
        "no_set": "-No set-",
        "new_set": "-New set-",
        "save_to_set": "Save to set",
        "new_set_name": "New set name",
        "must_log_in_to_save_to_set": "You must be logged in on We Heart It to save to a set."
      },
      "drop": {
        "drag_the_image_here": "Drag the image here to save it to your collection"
      }
    },
    "extension_specific": {
      "chrome": {
        "extension_name": "We Heart It",
        "extension_description": "Official We Heart It Google Chrome extension",
        "action_default_title": "Heart an image on this page!"
      },
      "safari": {
        "name": "We Heart It",
        "author": "Super Basic, LLC.",
        "description": "Save your favorite images on your weheartit account."
      },
      "firefox": {
        "name": "Heart Button",
        "description": "Heart Button from We Heart It (Super Basic, LLC.)",
        "creator": "We Heart It (Super Basic, LLC.)"
      }
    }
  },
  "es": {
    "bookmarklet": {
      "no_set": "-Sin galería-",
      "new_set": "-Nueva galería-",
      "new_set_name": "Nombre de la nueva galería",
      "must_log_in_to_save_to_set": "Debes iniciar sesión en We Heart It para guardar en una galería.",
      "no_image_we_can_heart": "No pudimos encontrar ninguna imagen para heartear en esta página.",
      "why_is_that_q": "¿Por qué es eso?",
      "you_can_only_heart_on_websites": "Sólo puedes heartear imágenes de sitios web, ",
      "not_directly": "no la imagen directamente en el navegador.",
      "go_back_to_site_and_heart": "Por favor, regresa al sitio y heartea desde allí.",
      "save_image_to_my_collection": "Guardar imagen en mi colección",
      "download_new_whi_button": "Descarga el nuevo Botón Heart It",
      "tap_the_image": "Toca la imagen que quieres heartear.",
      "move_your_mouse_over": "Mueve tu mouse sobre la imagen que quieres heartear.",
      "alert_no_heart": "Este sitio no permite heartear en We Heart It. Por favor, contacta al dueño si tienes preguntas. ¡Gracias por tu visita!",
      "alert_no_pin": "Este sitio no permite crear marcadores para sitios como We Heart It. Por favor, contacta al dueño si tienes preguntas. ¡Gracias por tu visita!",
      "alert_we_heart_it": "Estás tratando de usar el Botón Heart It en weheartit.com, pero en nuestro sitio solo necesitas hacer clic en el corazón sobre la imagen.",
      "need_help_q": "¿necesitas algo de ayuda con eso?",
      "alert_private": "Estás tratando de conseguir imágenes de una página a la cual sólo tú puedes acceder. Por favor, abre el enlace permanente en otra ventana y heartea desde allí.",
      "alert_public": "Las imágenes de %{website} generalmente son privadas.",
      "alert_whi_posting_public": "La publicación en We Heart It hará pública la imagen, y el nombre de quien la ha publicado en %{website} puede aparecer en el sitio.",
      "alert_confirm": "He leído y entendido",
      "alert_google_images": "No puedes heartear imágenes en la búsqueda de Google. Por favor, abre el link de la imagen y heartea desde allí.",
      "alert_blocked": "Este sitio fue bloqueado por weheartit.com. Si deseas apelar, por favor hazlo %{appeal_link}.",
      "appeal_here": "aquí",
      "blocked": "Bloqueado",
      "before_you_heart": "Antes de heartear...",
      "hey": "¡Oye!",
      "new_release_notice": "Acabamos de lanzar el nuevo Botón Heart It. Lo mejor es integrarlo con tu navegador como una extensión completa para que sea más fiable y funcione con todos los sitios que visitas y te gustan.",
      "please_upgrade": "¡Por favor, actualiza y sigue hearteando!",
      "learn_more_and_upgrade": "Conoce más y actualiza",
      "new_button_available": "¡Nuevo Botón Heart It disponible!",
      "close_button": "cerrar",
      "title": "título",
      "add_title": "agrega un título a esta imagen",
      "tags": "etiquetas",
      "separate_with_commas": "separar con comas",
      "close": "Cerrar",
      "click_to_heart": "Haz clic en las imágenes para heartear"
    },
    "extension": {
      "save_image_to_my_collection": "Guardar imagen en mi colección",
      "form": {
        "title": "Título",
        "add_title_to_this_image": "Agregar título a esta imagen",
        "add_tags": "Agregar etiquetas",
        "tags_describe_image": "Las etiquetas son palabras que describen la imagen",
        "tags_example": "Por ejemplo, Fotografía, Blanco y Negro, Chicas"
      },
      "sets": {
        "no_set": "-Sin galería-",
        "new_set": "-Nueva galería-",
        "save_to_set": "Guardar en galería",
        "new_set_name": "Nombre de la nueva galería",
        "must_log_in_to_save_to_set": "Debes iniciar sesión en We Heart It para guardar en una galería."
      },
      "drop": {
        "drag_the_image_here": "Arrastra la imagen aquí para guardarla en tu colección"
      }
    },
    "extension_specific": {
      "chrome": {
        "extension_name": "We Heart It",
        "extension_description": "Extensión We Heart It oficial para Google Chrome",
        "action_default_title": "¡Heartea una imagen en esta página!"
      },
      "safari": {
        "name": "We Heart It",
        "author": "Super Basic, LLC.",
        "description": "Guarda tus imágenes favoritas en tu cuenta de weheartit."
      },
      "firefox": {
        "name": "Botón Heart",
        "description": "Botón Heart de We Heart It (Super Basic, LLC.)",
        "creator": "We Heart It (Super Basic, LLC.)"
      }
    }
  },
  "fr": {
    "bookmarklet": {
      "no_set": "-pas de set-",
      "new_set": "- nouveau set -",
      "new_set_name": "Nom du set",
      "must_log_in_to_save_to_set": "Vous devez être connecté sur We Heart It pour enregistrer l'image dans le set.",
      "no_image_we_can_heart": "Nous n'avons pas trouvé d'image que vous pouvez collectionner sur cette page.",
      "why_is_that_q": "Pourquoi ?",
      "you_can_only_heart_on_websites": "Vous ne pouvez collectionner que des images provenant de sites,",
      "not_directly": "pas l'image directement dans le navigateur.",
      "go_back_to_site_and_heart": "Veuillez revenir sur le site et sélectionner l'image à partir de là.",
      "save_image_to_my_collection": "Enregistrer l'image dans ma galerie",
      "download_new_whi_button": "Télécharger le nouveau bouton Heart",
      "tap_the_image": "Sélectionnez une image",
      "move_your_mouse_over": "Positionnez la souris sur une image.",
      "alert_no_heart": "Ce site ne permet pas de collectionner des images. Veuillez contacter son propriétaire pour toute question. Merci de votre visite !",
      "alert_no_pin": "Ce site ne permet pas de collectionner des images.​ Veuillez contacter son propriétaire pour toute question.​ Merci de votre visite !",
      "alert_we_heart_it": "Vous essayez d'utiliser le bouton Heart sur weheartit.com, mais sur notre site il vous suffit de cliquer sur le bouton en forme de coeur sur l'image.",
      "need_help_q": "besoin d'aide ?",
      "alert_private": "Vous essayez d'obtenir des images d'une page à laquelle vous êtes le seul à pouvoir y accéder. Veuillez ouvrir le lien dans une autre fenêtre.",
      "alert_public": "Les images sur %{website} sont généralement privées.",
      "alert_whi_posting_public": "Ajouter cette image sur We Heart It va la rendre publique, et le nom de celui qui l'a publiée sur %{website} peut apparaître sur le site.",
      "alert_confirm": "J'ai lu et compris",
      "alert_google_images": "Vous ne pouvez pas sélectionner une image depuis Google. Ouvrez le site de l'image et sélectionnez-la à partir de là.",
      "alert_blocked": "Ce site a été bloqué par weheartit.com. Si vous souhaitez y accéder, dites-le nous %{appeal_link}.",
      "appeal_here": "ici",
      "blocked": "Bloqué",
      "before_you_heart": "Avant de collectionner l'image ...",
      "hey": "Hey !",
      "new_release_notice": "Nous venons de sortir notre nouveau bouton. Il s'intègre mieux à votre navigateur. Etant désormais une extension, il est plus fiable et fonctionne avec tous les sites que vous aimez.",
      "please_upgrade": "Veuillez mettre à jour !",
      "learn_more_and_upgrade": "En savoir plus et mettre à jour",
      "new_button_available": "Nouveau bouton Heart disponible !",
      "close_button": "fermer",
      "title": "titre",
      "add_title": "ajouter un titre à cette image",
      "tags": "tags",
      "separate_with_commas": "Séparez les tags par des virgules",
      "close": "Annuler",
      "click_to_heart": "cliquez sur les images pour les hearter"
    },
    "extension": {
      "save_image_to_my_collection": "Enregistrer l'image dans ma galerie",
      "form": {
        "title": "Titre",
        "add_title_to_this_image": "Ajouter un titre à cette image",
        "add_tags": "Ajouter un tag",
        "tags_describe_image": "Les tags servent à décrire l'image.",
        "tags_example": "Ex: Photographie, noir et blanc, fille"
      },
      "sets": {
        "no_set": "- pas de set -",
        "new_set": "- nouveau set -",
        "save_to_set": "Enregistrer dans le set",
        "new_set_name": "Nom du set",
        "must_log_in_to_save_to_set": "Vous devez être connecté sur We Heart It pour enregistrer l'image dans un set."
      },
      "drop": {
        "drag_the_image_here": "Faites glisser l'image ici pour l'enregistrer dans votre galerie"
      }
    },
    "extension_specific": {
      "chrome": {
        "extension_name": "We Heart It",
        "extension_description": "Extension de We Heart It pour Google Chrome",
        "action_default_title": "Sauvegardez une image de cette page !"
      },
      "safari": {
        "name": "We Heart It",
        "author": "Super Basic, LLC.",
        "description": "Enregistrez vos images préférées sur votre compte weheartit."
      },
      "firefox": {
        "name": "Bouton Heart",
        "description": "L'extension de We Heart It (Super Basic, LLC.)",
        "creator": "We Heart It (Super Basic, LLC.)"
      }
    }
  },
  "hu": {
    "bookmarklet": {
      "no_set": "-Nincs Gyűjtemény-",
      "new_set": "-Új gyűjtemény-",
      "new_set_name": "Új gyűjtemény címe",
      "must_log_in_to_save_to_set": "Gyűjteménybe mentéshez be kell jelentkezned a We Heart It-ra.",
      "no_image_we_can_heart": "Nem találtunk olyan képet, amit Szívelhetnél ezen az oldalon.",
      "why_is_that_q": "Miért van ez?",
      "you_can_only_heart_on_websites": "Csak weboldalakról tehetsz képeket a Szíveltjeid közé,",
      "not_directly": "nem a böngészőben közvetlenül megnyitott képet.",
      "go_back_to_site_and_heart": "Látogass vissza a webhelyre és onnan tedd a Szíveltjeid közé.",
      "save_image_to_my_collection": "Kép mentése gyűjteményembe.",
      "download_new_whi_button": "Töltsd le az új Szível gombot",
      "tap_the_image": "Koppints arra a képre, amit Szívelni akarsz.",
      "move_your_mouse_over": "Vidd az egeret a kép fölé, amit Szívelni akarsz.",
      "alert_no_heart": "Erről az oldalról nem Szívelhetsz a We Heart It oldalra. Bármilyen kérdéssel fordulj az oldal tulajdonosához. Köszönjük, hogy benéztél!",
      "alert_no_pin": "Ez az oldal nem teszi lehetővé a könyvjelzőzést olyan webhelyekre, mint a We Heart It. Bármilyen kérdéssel fordulj az oldal tulajdonosához. Köszönjük, hogy benéztél!",
      "alert_we_heart_it": "A Szível Gombot próbálod használni a weheartit.com oldalon, de oldalunkon csak a képen lévő Szívre tudsz kattintani.",
      "need_help_q": "segíthetünk ebben?",
      "alert_private": "Olyan oldalról próbálsz képeket letölteni, amelyhez csak neked van hozzáférésed. Nyisd meg a közvetlen linket egy új ablakban és onnan Szívelj.",
      "alert_public": "A képek %{website}-on általában privátok.",
      "alert_whi_posting_public": "A kép közzététele a We Heart It oldalon nyílvánossá teszi azt, és annak a neve, aki a %{website}-ra feltöltötte, megjelenhet az oldalon.",
      "alert_confirm": "Elolvastam és megértettem",
      "alert_google_images": "A Google Keresőből nem Szívelhetsz képeket. Nyisd meg a kép linkjét és onnan Szíveld.",
      "alert_blocked": "Ezt az oldalt a weheartit.com letiltotta. Ha ezt fel szeretnéd oldatni, tedd meg %{appeal_link}.",
      "appeal_here": "itt",
      "blocked": "Tiltva",
      "before_you_heart": "Mielőtt Szívelnél...",
      "hey": "Szia!",
      "new_release_notice": "Megjelent az új \"Szível\" gomb. Teljes kiterjesztésként jobban integrálódik a böngésződbe, így megbízhatóbb és minden oldalon használható, ahová szívesen látogatsz el.",
      "please_upgrade": "Frissíts és Szívelj továbbra is!",
      "learn_more_and_upgrade": "További tudnivalók és frissítés",
      "new_button_available": "Új \"Szível\" gomb érhető el!",
      "close_button": "bezárás",
      "title": "cím",
      "add_title": "adj címet ennek a képnek",
      "tags": "címkék",
      "separate_with_commas": "vesszőkkel válaszd el",
      "close": "Bezárás",
      "click_to_heart": "Szíveléshez kattints a képekre"
    },
    "extension": {
      "save_image_to_my_collection": "Kép mentése gyűjteményembe.",
      "form": {
        "title": "Cím",
        "add_title_to_this_image": "Adj címet ennek a képnek",
        "add_tags": "Címke hozzáadása",
        "tags_describe_image": "A címkék a képet leíró szavak.",
        "tags_example": "Pl.: Fotózás, Fekete és Fehér, Lány"
      },
      "sets": {
        "no_set": "-Nincs Gyűjtemény-",
        "new_set": "-Új gyűjtemény-",
        "save_to_set": "Mentés gyűjteménybe",
        "new_set_name": "Új gyűjtemény címe",
        "must_log_in_to_save_to_set": "Gyűjteménybe mentéshez be kell jelentkezned a We Heart It-ra."
      },
      "drop": {
        "drag_the_image_here": "Húzd a képet ide gyűjteményedbe való mentéshez"
      }
    },
    "extension_specific": {
      "chrome": {
        "extension_name": "We Heart It",
        "extension_description": "Hivatalos We Heart It Google Chrome kiterjesztés",
        "action_default_title": "Szívelj egy képet ezen az oldalon!"
      },
      "safari": {
        "name": "We Heart It",
        "author": "Super Basic, LLC.",
        "description": "Mentsd kedvenc képeidet a weheartit fiókodba."
      },
      "firefox": {
        "name": "Szível Gomb",
        "description": "Szível Gomb a We Heart It (Super Basic, LLC.)-tól",
        "creator": "We Heart It (Super Basic, LLC.)"
      }
    }
  },
  "id": {
    "bookmarklet": {
      "no_set": "-Tidak ada set-",
      "new_set": "-Set baru-",
      "new_set_name": "Nama set baru",
      "must_log_in_to_save_to_set": "Anda harus masuk ke We Heart It untuk menyimpan ke dalam set.",
      "no_image_we_can_heart": "Kami tidak dapat menemukan gambar apa pun yang dapat Anda heart di halaman ini.",
      "why_is_that_q": "Mengapa begitu?",
      "you_can_only_heart_on_websites": "Anda hanya dapat mengheart gambar dari situs web",
      "not_directly": "bukan gambar secara langsung di peramban.",
      "go_back_to_site_and_heart": "Silakan kembali ke situs dan heart dari sana.",
      "save_image_to_my_collection": "Simpan gambar ke koleksi saya",
      "download_new_whi_button": "Unduh Tombol Heart Baru",
      "tap_the_image": "Tekan pada gambar yang ingin Anda heart",
      "move_your_mouse_over": "Gerakkan mouse Anda ke atas gambar yang ingin Anda heart.",
      "alert_no_heart": "Situs ini tidak mengijinkan Anda mengheart di We Heart It. Silakan menghubungi pemiliknya jika ada pertanyaan. Terima kasih telah berkunjung!",
      "alert_no_pin": "Situs ini tidak mengijinkan bookmark ke situs-situs seperti We Heart It. Silakan menghubungi pemilik jika ada pertanyaan. Terima kasih telah berkunjung!",
      "alert_we_heart_it": "Anda tengah mencoba menggunakan Tombol Heart di weheartit.com, namun di situs kami, Anda cukup mengklik simbol heart di atas gambar.",
      "need_help_q": "butuh bantuan dengan itu?",
      "alert_private": "Anda mencoba mengambil gambar dari suatu halaman dimana hanya Anda yang dapat mengaksesnya. Silakan buka permalink di jendela lainnya dan heart dari sana.",
      "alert_public": "Gambar di %{website} umumnya bersifat pribadi. ",
      "alert_whi_posting_public": "Memposting gambar di We Heart It akan membuatnya menjadi gambar publik dan nama orang yang mempostingnya di %{website} akan muncul di situs tersebut.",
      "alert_confirm": "Saya telah membaca dan memahami",
      "alert_google_images": "Anda tidak dapat mengheart gambar lewat Mesin Pencari Google. Silakan buka tautan gambar dan heart dari sana.",
      "alert_blocked": "Situs ini telah diblokir oleh weheartit.com. Jika Anda ingin mengajukan banding, silakan lakukan %{appeal_link}.",
      "appeal_here": "di sini",
      "blocked": "Diblokir",
      "before_you_heart": "Sebelum Anda mengheart...",
      "hey": "Hai!",
      "new_release_notice": "Kami baru saja merilis Tombol Heart baru kami. Tombol ini sebaiknya diintegrasikan dengan peramban Anda sebagai ekstensi penuh sehingga lebih handal dan dapat bekerja pada semua situs yang Anda kunjungi dan sukai.",
      "please_upgrade": "Silakan upgrade dan teruslah mengheart!",
      "learn_more_and_upgrade": "Pelajari lebih lanjut dan perbarui",
      "new_button_available": "Tombol Heart baru tersedia!",
      "close_button": "tutup",
      "title": "judul",
      "add_title": "tambahkan judul ke gambar ini",
      "tags": "penanda",
      "separate_with_commas": "pisahkan dengan tanda koma",
      "close": "Tutup",
      "click_to_heart": "klik gambar untuk mengheart"
    },
    "extension": {
      "save_image_to_my_collection": "Simpan gambar ke koleksi saya",
      "form": {
        "title": "Judul",
        "add_title_to_this_image": "Tambah judul ke gambar ini",
        "add_tags": "Tambahkan penanda",
        "tags_describe_image": "Penanda adalah kata-kata yang mendeskripsikan gambar",
        "tags_example": "Misalnya: Fotografi, Hitam Putih, Perempuan"
      },
      "sets": {
        "no_set": "-Tidak ada set-",
        "new_set": "-Set baru-",
        "save_to_set": "Simpan ke set",
        "new_set_name": "Nama set baru",
        "must_log_in_to_save_to_set": "Anda harus masuk ke We Heart It untuk menyimpan ke dalam sebuah set"
      },
      "drop": {
        "drag_the_image_here": "Seret gambar ke sini untuk menyimpannya ke dalam koleksi Anda"
      }
    },
    "extension_specific": {
      "chrome": {
        "extension_name": "We Heart It",
        "extension_description": "Ekstensi Resmi We Heart It untuk Google Chrome",
        "action_default_title": "Heart sebuah gambar di halaman ini!"
      },
      "safari": {
        "name": "We Heart It",
        "author": "Super Basic, LLC.",
        "description": "Simpan gambar-gambar favorit di aku weheartit Anda."
      },
      "firefox": {
        "name": "Tombol Heart",
        "description": "Tombol Heart dari We Heart It (Super Basic, LLC.)",
        "creator": "We Heart It (Super Basic, LLC.                 "
      }
    }
  },
  "it": {
    "bookmarklet": {
      "no_set": "-Nessun gruppo-",
      "new_set": "-Nuovo gruppo-",
      "new_set_name": "Nome del nuovo gruppo",
      "must_log_in_to_save_to_set": "Devi essere già registrato/a su We Heart It per eseguire il salvataggio in un gruppo.",
      "no_image_we_can_heart": "In questa pagina non abbiamo potuto trovare immagini cui tu possa dare hearts. ",
      "why_is_that_q": "Perché tutto ciò?",
      "you_can_only_heart_on_websites": "Puoi dare hearts solo a immagini provenienti da siti web,",
      "not_directly": "non l'immagine direttamente nel browser.",
      "go_back_to_site_and_heart": "Per favore, ritorna al sito web e dai hearts da lì.",
      "save_image_to_my_collection": "Salva immagine nella mia collezione",
      "download_new_whi_button": "Scarica il nuovo Pulsante Heart",
      "tap_the_image": "Dai un colpettino sull'immagine che vuoi dotare di heart.",
      "move_your_mouse_over": "Fai scorrere il tuo mouse sull'immagine che vuoi dotare di heart.",
      "alert_no_heart": "Questo sito non consente di dare hearts per We Heart It. Per favore, contatta il suo proprietario per qualsiasi domanda. Grazie per averci fatto visita!",
      "alert_no_pin": "Questo sito non consente il bookmark verso siti quali We Heart It. Per favore, contatta il suo proprietario per qualsiasi domanda. Grazie per averci fatto visita!",
      "alert_we_heart_it": "Stai cercando di usare il Pulsante Heart su weheartit.com, ma nel nostro sito puoi solamente cliccare il simbolo dell'heart sovrapposto all'immagine.",
      "need_help_q": "hai bisogno di aiuto per questo?",
      "alert_private": "Stai cercando di procurarti immagini da una pagina alla quale solo tu hai accesso. Per favore, apri il permalink in un'altra finestra e dai hearts da lì.",
      "alert_public": "Le immagini su %{website} sono normalmente private.",
      "alert_whi_posting_public": "Postare l'immagine su We Heart It la renderà di dominio pubblico e il nome di chi l'ha postata su %{website} può apparire nel sito.",
      "alert_confirm": "Ho letto e compreso",
      "alert_google_images": "Non puoi attribuire hearts su Google Search. Per favore, apri il link dell'immagine e attribuisci l'heart da lì.",
      "alert_blocked": "Questo sito è stato bloccato da weheartit.com. Se vuoi richiedere un riesame del blocco, per favore proponilo pure %{appeal_link}.",
      "appeal_here": "qui",
      "blocked": "Bloccato",
      "before_you_heart": "Prima che tu dia un heart...",
      "hey": "Ehi!",
      "new_release_notice": "Abbiamo appena lanciato il nostro nuovo Pulsante Heart. Essendo meglio integrato nel tuo browser quale estensione completa, risulta quindi più affidabile  e funziona con tutti i siti che visiti e ami.",
      "please_upgrade": "Per favore, fai l'upgrade e continua a dare hearts!",
      "learn_more_and_upgrade": "Apprendi di più e fai l'upgrade",
      "new_button_available": "Disponibile il nuovo Pulsante Heart!",
      "close_button": "chiudi",
      "title": "titolo",
      "add_title": "aggiungi un titolo a questa immagine",
      "tags": "tags",
      "separate_with_commas": "separa con virgole",
      "close": "Chiudi",
      "click_to_heart": "clicca sulle immagini per dare hearts"
    },
    "extension": {
      "save_image_to_my_collection": "Salva immagine nella mia collezione",
      "form": {
        "title": "Titolo",
        "add_title_to_this_image": "Aggiungi un titolo a questa immagine",
        "add_tags": "Aggiungi tags",
        "tags_describe_image": "Le tags sono parole che descrivono l'immagine",
        "tags_example": "Ad esempio: Fotografia, Nero e Bianco, Ragazza"
      },
      "sets": {
        "no_set": "-Nessun gruppo-",
        "new_set": "-Nuovo gruppo-",
        "save_to_set": "Salva nel gruppo",
        "new_set_name": "Nome del nuovo gruppo",
        "must_log_in_to_save_to_set": "Devi avere fatto l'accesso a We Heart It per potere salvare in un gruppo."
      },
      "drop": {
        "drag_the_image_here": "Trascina l'immagine qui per salvarla nella tua collezione"
      }
    },
    "extension_specific": {
      "chrome": {
        "extension_name": "We Heart It",
        "extension_description": "Estensione ufficiale di Google Chrome per We Heart It",
        "action_default_title": "Attribuisci un heart a un'immagine su questa pagina!"
      },
      "safari": {
        "name": "We Heart It",
        "author": "Super Basic, LLC.",
        "description": "Salva le tue immagini preferite nel tuo account di weheartit."
      },
      "firefox": {
        "name": "Pulsante Heart",
        "description": "Pulsante Heart da We Heart It (Super Basic, LLC.)",
        "creator": "We Heart It (Super Basic, LLC.)"
      }
    }
  },
  "ja": {
    "bookmarklet": {
      "no_set": "-セットはありません-",
      "new_set": "- 新しいセット -",
      "new_set_name": "新しいセット名",
      "must_log_in_to_save_to_set": "セットに保存するには、We Heart It にログインする必要があります。",
      "no_image_we_can_heart": "このページには、ハートできる画像はありません。",
      "why_is_that_q": "これは何ですか？",
      "you_can_only_heart_on_websites": "ハートできるのはウェブサイトに掲載されている画像のみです。",
      "not_directly": "ブラウザ内に直接表示される画像ではありません。",
      "go_back_to_site_and_heart": "ウェブサイトにお戻りになり、ウェブサイトからハートしてください。",
      "save_image_to_my_collection": "コレクションに画像を保存",
      "download_new_whi_button": "新しいハート ボタンをダウンロード",
      "tap_the_image": "ハートしたい画像をタップします。",
      "move_your_mouse_over": "ハートしたい画像にカーソルを合わせます。",
      "alert_no_heart": "このサイトは、We Heart It への掲載を許可していません。ご質問がある場合は、サイト オーナーにご連絡ください。アクセスしていただき、ありがとうございます！",
      "alert_no_pin": "このサイトは、We Heart It のようなサイトへのブックマーク登録を許可していません。ご質問がある場合は、サイト オーナーにご連絡ください。アクセスしていただき、ありがとうございます！",
      "alert_we_heart_it": "weheartit.com でハート ボタンをご利用になろうとしていますが、このサイトでは画像上でハート ボタンをクリックするだけで同様の操作を行うことができます。",
      "need_help_q": "サポートが必要ですか？",
      "alert_private": "あなたのみがアクセス権を持つページから画像を取得しようとしています。別のウィンドウで固定リンクを開き、そこからハートしてください。",
      "alert_public": "通常、%{website} の画像は限定公開です。",
      "alert_whi_posting_public": "We Heart It へ投稿することにより画像が公開され、%{website} にその画像を投稿したユーザーの名前が We Heart It サイトに表示される可能性があります。",
      "alert_confirm": "全文を読み、その内容を理解しました",
      "alert_google_images": "Google 検索上の画像をハートすることはできません。画像のリンクを開き、そこからハートしてください。",
      "alert_blocked": "このサイトは、weheartit.com でブロックされています。申し立てする場合は、%{appeal_link}。",
      "appeal_here": "こちらから行ってください。",
      "blocked": "ブロックされています",
      "before_you_heart": "ハートする前に...",
      "hey": "こんにちは！",
      "new_release_notice": "新しいハート ボタンをリリースしました。完全な拡張機能として、ブラウザとの統合性が高まりました。信頼性が高まり、アクセスするすべてのサイトに対応します。",
      "please_upgrade": "アップグレードして、どんどんハートしよう！",
      "learn_more_and_upgrade": "詳細およびアップグレード",
      "new_button_available": "新しいハート ボタンをチェック！",
      "close_button": "閉じる",
      "title": "タイトル",
      "add_title": "この画像にタイトルを追加",
      "tags": "タグ",
      "separate_with_commas": "カンマで区切る",
      "close": "閉じる",
      "click_to_heart": "画像をクリックしてハートする"
    },
    "extension": {
      "save_image_to_my_collection": "コレクションに画像を保存",
      "form": {
        "title": "タイトル",
        "add_title_to_this_image": "この画像にタイトルを追加",
        "add_tags": "タグを追加",
        "tags_describe_image": "タグは画像を説明するキーワードです",
        "tags_example": "例: 写真撮影、白黒、女の子"
      },
      "sets": {
        "no_set": "- セットはありません -",
        "new_set": "- 新しいセット -",
        "save_to_set": "セットに保存",
        "new_set_name": "新しいセット名",
        "must_log_in_to_save_to_set": "セットに保存するには、We Heart It にログインする必要があります。"
      },
      "drop": {
        "drag_the_image_here": "ここに画像をドラッグしてコレクションに保存する"
      }
    },
    "extension_specific": {
      "chrome": {
        "extension_name": "We Heart It",
        "extension_description": "We Heart It の公式 Google Chrome 拡張機能",
        "action_default_title": "このページで画像をハートしよう！"
      },
      "safari": {
        "name": "We Heart It",
        "author": "Super Basic, LLC.",
        "description": "weheartit アカウントにお気に入りの画像を保存します。"
      },
      "firefox": {
        "name": "ハートボタン",
        "description": "We Heart It（Super Basic, LLC.）のハートボタン",
        "creator": "We Heart It（Super Basic, LLC.）"
      }
    }
  },
  "ko": {
    "bookmarklet": {
      "no_set": "- 세트 없이 - ",
      "new_set": "-새로운 세트-",
      "new_set_name": "새로운 세트 이름",
      "must_log_in_to_save_to_set": "세트를 저장하려면 We Heart It에 로그인해야 합니다.",
      "no_image_we_can_heart": "이 페이지에서는 하트로 찜할 수 있는 이미지를 찾을 수가 없어요.",
      "why_is_that_q": "왜 그렇죠?",
      "you_can_only_heart_on_websites": "웹사이트에서 가져온 이미지만 하트로 찜할 수 있습니다,",
      "not_directly": "브라우저에서 직접 이미지하지 않기",
      "go_back_to_site_and_heart": "사이트로 돌아가서 하트로 찜하도록 하세요. ",
      "save_image_to_my_collection": "내 컬렉션에 이미지 저장하기",
      "download_new_whi_button": "새로운 하트 버튼을 다운로드 받으세요",
      "tap_the_image": "하트로 찜하고 싶은 이미지에 탭하면 됩니다. ",
      "move_your_mouse_over": "하트로 찜하고 싶은 이미지 위로 마우스를 움직이세요. ",
      "alert_no_heart": "이 사이트는 We Heart It에 하트로 찜하도록 허용하고 있지 않습니다. 사이트 소유자에게 문의하도록 하세요. 방문해주셔서 감사합니다!",
      "alert_no_pin": "이 사이트는 We Heart It와 같은 사이트를 즐겨찾기 추가에 허용하고 있지 않습니다. 사이트 소유자에게 문의하도록 하세요. 방문해주셔서 감사합니다!",
      "alert_we_heart_it": "weheartit.com에서 하트 버튼을 이용하려고 시도중이네요. 그러나 여기서는 그냥 이미지 위의 하트 심볼을 클릭만 하면 된답니다. ",
      "need_help_q": "도움이 필요하신가요?",
      "alert_private": "접근 권한이 있는 페이지에서 이미지를 받으려하고 있습니다. 영구 링크를 다른 창에서 열어서 거기서 하트로 찜해주세요.",
      "alert_public": "%{website}의 이미지는 일반적으로 비공개입니다.",
      "alert_whi_posting_public": "We Heart It에 올리게 될 경우 이미지가 공개되며 누가 %{website}에 올렸는지 이름이 사이트에 게재될 수 있습니다. ",
      "alert_confirm": "읽고 이해했습니다",
      "alert_google_images": "구글 검색에서는 이미지를 하트로 찜할 수가 없습니다. 이미지 링크를 열고 사이트에서 하트로 찜해주세요. ",
      "alert_blocked": "이 사이트는 weheartit.com으로 부터 차단되었습니다. 항소를 원하신다면, 문의하세요 %{appeal_link}.",
      "appeal_here": "여기에",
      "blocked": "차단되었습니다",
      "before_you_heart": "하트로 찜하기 전에...",
      "hey": "안녕하세요!",
      "new_release_notice": "새로운 하트 버튼이 방금 출시되었습니다. 더 나은 전체 확장 기능으로 브라우저와 통합되서 더 신뢰할 수 있고 내가 방문하고 좋아하는 모든 사이트에서 사용할 수 있어요. ",
      "please_upgrade": "업그레이드하고 계속 하트로 찜하세요!",
      "learn_more_and_upgrade": "더 자세히 알아보고 업그레이드하기",
      "new_button_available": "새로운 하트 버튼이 나왔어요!",
      "close_button": "닫기",
      "title": "제목",
      "add_title": "이미지에 제목 정하기",
      "tags": "태그",
      "separate_with_commas": "쉼표로 구분하기",
      "close": "닫기",
      "click_to_heart": "하트로 찜하려면 이미지를 클릭하세요"
    },
    "extension": {
      "save_image_to_my_collection": "내 컬렉션에 이미지 저장하기",
      "form": {
        "title": "제목",
        "add_title_to_this_image": "이미지에 제목 정하기",
        "add_tags": "태그 추가하기",
        "tags_describe_image": "태그는 이미지를 설명하는 키워드입니다",
        "tags_example": "예: 사진, 흑과 백, 소녀"
      },
      "sets": {
        "no_set": "- 세트 없이 - ",
        "new_set": "-새로운 세트-",
        "save_to_set": "세트에 저장하기",
        "new_set_name": "새로운 세트 이름",
        "must_log_in_to_save_to_set": "세트를 저장하려면 We Heart It에 로그인해야 합니다."
      },
      "drop": {
        "drag_the_image_here": "컬렉션에 저장하려면 이미지를 여기에 드래그하세요"
      }
    },
    "extension_specific": {
      "chrome": {
        "extension_name": "We Heart It",
        "extension_description": "공식 We Heart It 구글 크롬 확장 프로그램",
        "action_default_title": "이 페이지의 이미지를 하트로 찜하세요!"
      },
      "safari": {
        "name": "We Heart It",
        "author": "Super Basic, LLC.",
        "description": "weheartit 계정에 좋아하는 이미지를 저장하세요. "
      },
      "firefox": {
        "name": "하트 버튼",
        "description": "We Heart It (Super Basic, LLC.)의 하트 버튼",
        "creator": "We Heart It (Super Basic, LLC.)"
      }
    }
  },
  "nl": {
    "bookmarklet": {
      "no_set": "-Geen Label-",
      "new_set": "-Nieuwe collectie-",
      "new_set_name": "Nieuwe collectienaam ",
      "must_log_in_to_save_to_set": "Je moet aangemeld zijn op We Heart It om te kunnen opslaan in een collectie. ",
      "no_image_we_can_heart": "We hebben geen afbeeldingen gevonden die je kunt harten op deze pagina. ",
      "why_is_that_q": "Waarom is dat? ",
      "you_can_only_heart_on_websites": "Je kunt alleen afbeeldingen harten van websites, ",
      "not_directly": "niet de afbeelding direct in de browser. ",
      "go_back_to_site_and_heart": "Ga terug naar de website en hart daar. ",
      "save_image_to_my_collection": "Afbeelding opslaan in mijn collectie",
      "download_new_whi_button": "Download de nieuwe Hart Button",
      "tap_the_image": "Tik op de afbeelding die je wilt harten. ",
      "move_your_mouse_over": "Beweeg je muis over de afbeelding die je wilt harten. ",
      "alert_no_heart": "Deze website staat niet toe om te harten naar We Heart It. Neem contact op met de eigenaar als je vragen hebt. Bedankt voor het bezoeken!",
      "alert_no_pin": "Deze website staat bladwijzeren naar websites zoals We Heart It niet toe. Neem contact op met de eigenaar als je vragen hebt. Bedankt voor het bezoeken!",
      "alert_we_heart_it": "Je probeert de Hart Button te gebruiken op weheartit.com, maar op onze site kun je gewoon op het hart symbool in de afbeelding klikken. ",
      "need_help_q": "heb je er wat hulp bij nodig?",
      "alert_private": "Je probeert afbeeldingen van een pagina te halen waar alleen jij toegang tot hebt. Open de permalink in een ander venster en hart vanaf daar. ",
      "alert_public": "Afbeeldingen op %{website} zijn over het algemeen privé. ",
      "alert_whi_posting_public": "Plaatsen op We Heart It zorgt ervoor dat de afbeelding publiek wordt en de naam van degene die het geplaatst heeft op %{website} kan verschijnen op de site. ",
      "alert_confirm": "Ik heb het gelezen en begrepen",
      "alert_google_images": "Je kunt geen afbeeldingen op Google Search harten. Open de link van de afbeelding en hart vanaf daar. ",
      "alert_blocked": "Deze site is geblokkeerd door weheartit.com. Als je daar tegenin wilt gaan, doe dat alsjeblieft%{appeal_link}. ",
      "appeal_here": "hier",
      "blocked": "Geblokkeerd",
      "before_you_heart": "Voordat je hart...",
      "hey": "Hey!",
      "new_release_notice": "We hebben zojuist onze nieuwe Hart Button uitgebracht. Deze is  als een volledige extensie beter geïntegreerd met je browser en is daarom betrouwbaarder en werkt op alle websites waar je van houdt en die je bezoekt. ",
      "please_upgrade": "Upgrade en blijft harten!",
      "learn_more_and_upgrade": "Leer meer en upgrade",
      "new_button_available": "Nieuwe Hart Button beschikbaar!",
      "close_button": "sluiten",
      "title": "titel",
      "add_title": "voeg een titel toe aan deze afbeelding",
      "tags": "labels",
      "separate_with_commas": "splits met komma's ",
      "close": "Sluit",
      "click_to_heart": "klik afbeeldingen om te harten"
    },
    "extension": {
      "save_image_to_my_collection": "Bewaar afbeelding in mijn collectie",
      "form": {
        "title": "Titel",
        "add_title_to_this_image": "Voeg een titel toe aan deze afbeelding",
        "add_tags": "Labels toevoegen",
        "tags_describe_image": "Labels zijn woorden die de afbeelding beschrijven",
        "tags_example": "Bv.: Fotografie, Zwart en Wit, Meisje"
      },
      "sets": {
        "no_set": "-Geen collectie-",
        "new_set": "-Nieuwe collectie-",
        "save_to_set": "Bewaar in collectie",
        "new_set_name": "Nieuwe collectie naam",
        "must_log_in_to_save_to_set": "Je moet aangemeld zijn bij We Heart It om op te kunnen slaan in een collectie. "
      },
      "drop": {
        "drag_the_image_here": "Sleep de afbeelding hier om deze op te slaan in je collectie"
      }
    },
    "extension_specific": {
      "chrome": {
        "extension_name": "We Heart It",
        "extension_description": "Officiële We Heart It Google Chrome extensie ",
        "action_default_title": "Hart een afbeelding op deze pagina! "
      },
      "safari": {
        "name": "We Heart It",
        "author": "Super Basic, LLC.",
        "description": "Bewaar je favoriete afbeeldingen op je weheartit account. "
      },
      "firefox": {
        "name": "Hart Button ",
        "description": "Hart Button van We Heart It (Super Basic, LLC.)",
        "creator": "We Heart It (Super Basic, LLC.)"
      }
    }
  },
  "pl": {
    "bookmarklet": {
      "no_set": "-Bez zestawu-",
      "new_set": "-Nowy zestaw-",
      "new_set_name": "Nazwa zestawu",
      "must_log_in_to_save_to_set": "Musisz zalogować się na We Heart It żeby zapisywać do zestawu.",
      "no_image_we_can_heart": "Nie znaleźliśmy na tej stronie obrazków, które można zaserduszkować.",
      "why_is_that_q": "Dlaczego?",
      "you_can_only_heart_on_websites": "Możesz serduszkować obrazki jedynie ze stron internetowych,",
      "not_directly": "nie obrazek bezpośrednio w przeglądarce.",
      "go_back_to_site_and_heart": "Proszę, wróć na stronę i serduszkuj stamtąd.",
      "save_image_to_my_collection": "Zapisz obrazek w mojej kolekcji",
      "download_new_whi_button": "Pobierz nowe Serduszko",
      "tap_the_image": "Puknij na obrazek, który chcesz zaserduszkować.",
      "move_your_mouse_over": "Najedź kursorem na obrazek, który chcesz zaserduszkować.",
      "alert_no_heart": "Ta strona nie pozwala na serduszkowanie do We Heart It. Jeśli masz jakieś pytania, skontaktuj się z właścicielem. Dziękujemy za odwiedziny!",
      "alert_no_pin": "Ta strona nie pozwala na dodawanie do stron takich jak We Heart It. Jeśli masz jakieś pytania, skontaktuj się z właścicielem. Dziękujemy za odwiedziny!",
      "alert_we_heart_it": "Próbujesz użyć zainstalowane Serduszko na weheartit.com, ale na naszej stronie wystarczy kliknąć symbol serduszka nad obrazkiem.",
      "need_help_q": "Potrzebujesz pomocy?",
      "alert_private": "Próbujesz dodać obrazek ze strony, do której tylko ty masz dostęp. Prosimy, otwórz odnośnik bezpośredni w innym okienku i to w nim zaserduszkuj.",
      "alert_public": "Obrazki na %{website} są zwykle prywatne.",
      "alert_whi_posting_public": "Publikowanie na We Heart It upublicznia obrazek, a imię osoby, która go opublikowała na %{website}, może pojawić się na stronie.",
      "alert_confirm": "Przeczytałem/am i zrozumiałem/am",
      "alert_google_images": "Nie możesz serduszkować obrazków w wyszukiwaniu Google. Otwórz odnośnik do obrazka i serduszkuj stamtąd.",
      "alert_blocked": "Ta strona została zablokowana przez weheartit.com. Możesz jednak wnieść prośbę o usunięcie blokady %{appeal_link}.",
      "appeal_here": "tutaj",
      "blocked": "Zablokowane",
      "before_you_heart": "Zanim zaserduszkujesz...",
      "hey": "Hej!",
      "new_release_notice": "Właśnie wydaliśmy nasze nowe Serduszko. Jest lepiej zintegrowane z przeglądarką jako pełne rozszerzenie, więc jest bardziej wydajne i działa na wszystkich stronach, które odwiedzasz i kochasz.",
      "please_upgrade": "Aktualizuj i serduszkuj dalej!",
      "learn_more_and_upgrade": "Dowiedz się więcej i upgrade'uj",
      "new_button_available": "Nowe Serduszko jest już dostępne!",
      "close_button": "zamknij",
      "title": "tytuł",
      "add_title": "dodaj tytuł do tego obrazka",
      "tags": "tagi",
      "separate_with_commas": "oddziel przecinkami",
      "close": "Zamknij",
      "click_to_heart": "klikaj obrazki żeby je serduszkować"
    },
    "extension": {
      "save_image_to_my_collection": "Zapisz obrazek w mojej kolekcji",
      "form": {
        "title": "Tytuł",
        "add_title_to_this_image": "Dodaj tytuł do tego obrazka",
        "add_tags": "Dodaj tagi",
        "tags_describe_image": "Tagi to słowa opisujące obrazek",
        "tags_example": "Np. Fotografia, Czarno-Białe, Dziewczyna"
      },
      "sets": {
        "no_set": "-Bez zestawu-",
        "new_set": "-Nowy zestaw-",
        "save_to_set": "Zapisz do zestawu",
        "new_set_name": "Nazwa nowego zestawu",
        "must_log_in_to_save_to_set": "Musisz zalogować się na We Heart It żeby zapisywać do zestawu."
      },
      "drop": {
        "drag_the_image_here": "Przeciągnij tutaj obrazek żeby zapisać go w swojej kolekcji"
      }
    },
    "extension_specific": {
      "chrome": {
        "extension_name": "We Heart It",
        "extension_description": "Oficjalne rozszerzenie We Heart It do Google Chrome",
        "action_default_title": "Zaserduszkuj obrazek na tej stronie!"
      },
      "safari": {
        "name": "We Heart It",
        "author": "Super Basic, LLC.",
        "description": "Zapisuj swoje ulubione obrazki na koncie weheartit."
      },
      "firefox": {
        "name": "Serduszko",
        "description": "Serduszko z We Heart It (Super Basic, LLC.)",
        "creator": "We Heart It (Super Basic, LLC.)"
      }
    }
  },
  "pt-br": {
    "bookmarklet": {
      "no_set": "-Nenhum set-",
      "new_set": "-Novo set-",
      "new_set_name": "Nome do set",
      "must_log_in_to_save_to_set": "Você deve estar logado no We Heart It para salvar em um set.",
      "no_image_we_can_heart": "Não encontramos nenhuma imagem que você possa heartear nessa página.",
      "why_is_that_q": "Por que isso?",
      "you_can_only_heart_on_websites": "Você só pode heartear imagens de websites,",
      "not_directly": "não a imagem diretamente no navegador.",
      "go_back_to_site_and_heart": "Por favor volte ao site e tente heartear de lá.",
      "save_image_to_my_collection": "Salvar imagem em minha coleção",
      "download_new_whi_button": "Baixe o novo Heart Button",
      "tap_the_image": "Selecione a imagem que deseja heartear.",
      "move_your_mouse_over": "Mova seu mouse sobre a imagem que deseja heartear.",
      "alert_no_heart": "Esse site não permite que imagens sejam hearteadas. Por favor contate o dono do site com quaiser perguntas. Obrigado!",
      "alert_no_pin": "Esse site não permite que imagens sejam salvas em sites como o We Heart It. Por favor contate o dono do site se tiver perguntas. Obrigado!",
      "alert_we_heart_it": "Você está tentando usar o Heart Button no weheartit.com, mas em nosso site você pode clicar no coração sobre a imagem para heartear.",
      "need_help_q": "precisa de ajuda com isso?",
      "alert_private": "Você está tentando heartear imagens de uma página que só você tem acesso. Por favor abra o post em outra janela e tente heartear de lá.",
      "alert_public": "Imagens de %{website} são geralmente privadas.",
      "alert_whi_posting_public": "Postar a imagem no We Heart It tornará ela pública e o nome de quem postou no %{website} pode aparecer no site.",
      "alert_confirm": "Eu li e compreendi",
      "alert_google_images": "Não é possível heartear imagens da busca do Google. Por favor abra o link da imagem e hearteie do site original.",
      "alert_blocked": "Esse site foi bloqueado pelo weheartit.com. Se você gostaria que ele fosse revisado, por favor entre em contato %{appeal_link}.",
      "appeal_here": "aqui",
      "blocked": "Bloqueado",
      "before_you_heart": "Antes de Heartear…",
      "hey": "Ei!",
      "new_release_notice": "Nós lançamos nosso novo Heart Button. Ele é melhor integrado com seu navegador como uma extensão, então é muito mais confiável e funciona com todos os sites que você visita e ama.",
      "please_upgrade": "Por favor faça o upgrade e continue hearteando!",
      "learn_more_and_upgrade": "Saiba mais e atualize",
      "new_button_available": "Novo Heart Button disponível!",
      "close_button": "fechar",
      "title": "título",
      "add_title": "adicione um título à imagem",
      "tags": "tags",
      "separate_with_commas": "separe com vírgulas",
      "close": "Fechar",
      "click_to_heart": "clique imagens para heartear"
    },
    "extension": {
      "save_image_to_my_collection": "Salvar imagem em minha coleção",
      "form": {
        "title": "Título",
        "add_title_to_this_image": "Adicione um título a essa imagem",
        "add_tags": "Adicionar tags",
        "tags_describe_image": "Tags são palavras que descrevem a imagem",
        "tags_example": "Ex: Fotografia, Preto e Branco, Menina"
      },
      "sets": {
        "no_set": "-Nenhum set-",
        "new_set": "-Novo set-",
        "save_to_set": "Salvar em um set",
        "new_set_name": "Nome do set",
        "must_log_in_to_save_to_set": "Você deve estar logado no We Heart It para salvar em um set."
      },
      "drop": {
        "drag_the_image_here": "Arraste a imagem aqui para salvá-la em sua coleção"
      }
    },
    "extension_specific": {
      "chrome": {
        "extension_name": "We Heart It",
        "extension_description": "Extensão oficial do We Heart It para Google Chrome",
        "action_default_title": "Hearteie uma imagem nessa página!"
      },
      "safari": {
        "name": "We Heart It",
        "author": "Super Basic, LLC.",
        "description": "Salve suas imagens favoritas em sua conta do We Heart It"
      },
      "firefox": {
        "name": "Heart Button",
        "description": "Heart Button de We Heart It (Super Basic, LLC.)",
        "creator": "We Heart It (Super Basic, LLC.)"
      }
    }
  },
  "ro": {
    "bookmarklet": {
      "no_set": "Nesetat",
      "new_set": "- Serie nouă -",
      "new_set_name": "Numele seriei noi",
      "must_log_in_to_save_to_set": "Trebuie să fii logată în contul We Heart It pentru a salva la serie.",
      "no_image_we_can_heart": "Nu am găsit nicio imagine, pe care o poţi marca de pe această pagină",
      "why_is_that_q": "De ce apare aceasta?",
      "you_can_only_heart_on_websites": "Poţi marca imagini doar de pe site-uri web,",
      "not_directly": "nu este imaginea directă în browser",
      "go_back_to_site_and_heart": "Te rugăm să revii pe site şi să marchezi imagini de acolo",
      "save_image_to_my_collection": "Salvează imaginea la colecţia mea",
      "download_new_whi_button": "Descarcă noul Heart Button",
      "tap_the_image": "Apasă pe imaginea, pe care doreşti să o marchezi",
      "move_your_mouse_over": "Mişcă mouse-ul pe imaginea, pe care doreşti să o marchezi",
      "alert_no_heart": "Acest website nu permite adăugarea imaginilor pe We Heart It. Te rugăm să contactezi proprietarul, dacă ai întrebări. Mulţumim pentru vizită!",
      "alert_no_pin": "Acest site nu permite marcajul către site-uri web ca We Heart It. Te rugăm să contactezi proprietarul, dacă ai întrebări. Mulţumim pentru vizită! ",
      "alert_we_heart_it": "Încerci să utilizezi Heart Button pe weheartit.com, însă pe site-ul nostru, trebuie doar să faci un click pe simbolul de inimă de pe imagine. ",
      "need_help_q": "Ai nevoie de ajutor?",
      "alert_private": "Încerci să adaugi imagini de pe pagina, la care doar tu ai acces. Te rugăm să deschizi permalink-ul într-o fereastră nouă şi să marchezi imagini de acolo.",
      "alert_public": "Imaginile de pe %{website} de regulă sunt private.",
      "alert_whi_posting_public": "Postarea acesteia pe We Heart It o va face publică, iar numele celui, care a postat-o pe %{website} poate apărea pe site. ",
      "alert_confirm": "Am citit şi înţeles",
      "alert_google_images": "Nu poţi adăuga imagini din Google Search. Te rugăm să deschizi link-ul imaginii şi să o marchezi de acolo.",
      "alert_blocked": "Acest site a fost blocat de weheartit.com, dacă doreşti să-l accesezi, te rugăm să continui %{appeal_link}.",
      "appeal_here": "aici",
      "blocked": "Blocat",
      "before_you_heart": "Înainte de a marca...",
      "hey": "Salut!",
      "new_release_notice": "Am lansat recent noul marcator - Heart Button. El este integrat bine în browser-ul pe care îl foloseşti şi lucrează ca o extensie completă, astfel acesta este un instrument sigur şi compatibil cu toate site-urile web pe care le preferi.",
      "please_upgrade": "Te rugăm să upgradezi şi să continui marcarea imaginilor!",
      "learn_more_and_upgrade": "Citeşte mai mult şi upgradează",
      "new_button_available": "Noul Heart Button este accesibil!",
      "close_button": "închide",
      "title": "titlu",
      "add_title": "adaugă un titlu la imaginea aceasta",
      "tags": "etichete",
      "separate_with_commas": "separă prin virgule",
      "close": "Închide",
      "click_to_heart": "apasă pe imagini pentru a le marca"
    },
    "extension": {
      "save_image_to_my_collection": "Salvează imaginea la colecţia mea",
      "form": {
        "title": "Titlu",
        "add_title_to_this_image": "Adaugă titlu la această imagine",
        "add_tags": "Adaugă etichete",
        "tags_describe_image": "Etichetele sunt cuvintele, ce descriu imaginea",
        "tags_example": "E.g.:Fotografie, Alb Negru, Fată"
      },
      "sets": {
        "no_set": "-Nicio serie-",
        "new_set": "-Serie nouă-",
        "save_to_set": "Salvează la serie",
        "new_set_name": "Numele seriei noi",
        "must_log_in_to_save_to_set": "Trebuie să te loghezi pe We Heart It pentru a salva în serie."
      },
      "drop": {
        "drag_the_image_here": "Trage imaginea aici pentru a o salva la colecţia ta"
      }
    },
    "extension_specific": {
      "chrome": {
        "extension_name": "We Heart It",
        "extension_description": "Extensia oficială We Heart It pe Google Chrome",
        "action_default_title": "Marchează o imagine de pe această pagină!"
      },
      "safari": {
        "name": "We Heart It",
        "author": "Super Basic, LLC.",
        "description": "Salvează imaginile tale preferate în contul weheartit."
      },
      "firefox": {
        "name": "Heart Button",
        "description": "Heart Button de la We Heart It (Super Basic, LLC.)",
        "creator": "We Heart It (Super Basic, LLC.)"
      }
    }
  },
  "ru": {
    "bookmarklet": {
      "no_set": "-Без сборника-",
      "new_set": "-Новый сборник-",
      "new_set_name": "Название нового сборника",
      "must_log_in_to_save_to_set": "Чтобы добавлять картинки в сборники, войди в свой We Heart It аккаунт.",
      "no_image_we_can_heart": "На этой странице не удалось найти картинки, которые можно отметить​.",
      "why_is_that_q": "Почему?",
      "you_can_only_heart_on_websites": "Отмечать картинки можно только на сайтах.",
      "not_directly": "а не картинку в браузере.",
      "go_back_to_site_and_heart": "Вернись на сайт и отметь картинку сердечком там.",
      "save_image_to_my_collection": "Сохранить картинку в моей коллекции",
      "download_new_whi_button": "Загрузить новую кнопку-сердечко",
      "tap_the_image": "Нажми на картинку, которую ты хочешь отметить сердечком.",
      "move_your_mouse_over": "Наведи курсор на картинку, которую ты хочешь отметить сердечком.",
      "alert_no_heart": "Этот сайт не разрешает отмечать материалы сердечками в We Heart It. По всем вопросам обращайтесь к владельцу ресурса. Спасибо за интерес к WHI!",
      "alert_no_pin": "Этот сайт не разрешает We Heart It и подобным системам создавать закладки. По всем вопросам обращайтесь к владельцу ресурса. Спасибо за интерес к WHI!",
      "alert_we_heart_it": "Ты используешь кнопку-сердечко на сайте weheartit.com, но чтобы отметить понравившуюся картинку на нашем сайте, достаточно нажать сердечко на ней.",
      "need_help_q": "нужна помощь?",
      "alert_private": "Ты пытаешься отметить картинки на странице, доступ к которой есть только у тебя. Чтобы отметить картинку, открой изображение в новом окне, используя прямую ссылку на него.",
      "alert_public": "Изображения на %{website}, как правило, частные.",
      "alert_whi_posting_public": "Опубликованная в We Heart It картинка станет общедоступной. Кроме того, на сайте может появиться имя ее автора из %{website}.",
      "alert_confirm": "С вышеизложенным ознакомлен(а)",
      "alert_google_images": "Дарить сердечки на странице результатов поиска Google невозможно. Сначала необходимо открыть ссылку на страницу с картинкой.",
      "alert_blocked": "Этот сайт был заблокирован weheartit.com. Оспорить это ты можешь %{appeal_link}.",
      "appeal_here": "здесь",
      "blocked": "Заблокирован",
      "before_you_heart": "Прочти, прежде чем начать...",
      "hey": "Привет!",
      "new_release_notice": "Мы только что выпустили новую версию кнопки-сердечка. Теперь это более надёжное полнофункциональное расширение для браузера, которое работает на всех сайтах.",
      "please_upgrade": "    Выполни обновление и продолжай дарить сердечки!    ",
      "learn_more_and_upgrade": "Подробнее об обновлении...",
      "new_button_available": "    Новая версия кнопки-сердечка!    ",
      "close_button": "закрыть",
      "title": "имя",
      "add_title": "добавить название картинки",
      "tags": "теги",
      "separate_with_commas": "несколько тегов? раздели их запятыми",
      "close": "Закрыть",
      "click_to_heart": "нажимай на картинки, чтобы отметить их сердечком."
    },
    "extension": {
      "save_image_to_my_collection": "Сохранить картинку в моей коллекции",
      "form": {
        "title": "Название",
        "add_title_to_this_image": "Добавить название картинки",
        "add_tags": "Добавить теги",
        "tags_describe_image": "Теги используются для описания картинок",
        "tags_example": "Например: Фото, Черно-белое, Девушка"
      },
      "sets": {
        "no_set": "-Без сборника-",
        "new_set": "-Новый сборник-",
        "save_to_set": "Сохранить в сборник",
        "new_set_name": "Имя нового сборника",
        "must_log_in_to_save_to_set": "Чтобы добавлять картинки в сборники, войди в свой We Heart It аккаунт."
      },
      "drop": {
        "drag_the_image_here": "Перетяни картинку сюда, чтобы добавить ее в коллекцию"
      }
    },
    "extension_specific": {
      "chrome": {
        "extension_name": "We Heart It",
        "extension_description": "Официальное расширение для Google Chrome от We Heart It",
        "action_default_title": "Отметить картинку на этой странице."
      },
      "safari": {
        "name": "We Heart It",
        "author": "Super Basic, LLC.",
        "description": "Добавляй понравившиеся картинки на We Heart It!"
      },
      "firefox": {
        "name": "Кнопка-сердечко",
        "description": "Кнопка-сердечко от We Heart It (Super Basic, LLC.)",
        "creator": "We Heart It (Super Basic, LLC.)"
      }
    }
  },
  "th": {
    "bookmarklet": {
      "no_set": "-ไม่มีกลุ่มภาพ-",
      "new_set": "-กลุ่มภาพใหม่-",
      "new_set_name": "ชื่อกลุ่มภาพใหม่",
      "must_log_in_to_save_to_set": "คุณต้องล็อกอินที่ We Heart It ถึงจะบันทึกกลุ่มภาพได้",
      "no_image_we_can_heart": "เราไม่พบรูปภาพที่คุณสามารถกด Heart ได้บนหน้าเว็บนี้",
      "why_is_that_q": "ทำไมนะ?",
      "you_can_only_heart_on_websites": "คุณสามารถกด Heart รูปภาพจากเว็บไซต์เท่านั้น",
      "not_directly": "ไม่ใช่รูปภาพในเบราว์เซอร์โดยตรง",
      "go_back_to_site_and_heart": "กรุณากลับไปที่เว็บไซต์และกด Heart จากที่นั่น",
      "save_image_to_my_collection": "บันทึกรูปภาพไปยังคอลเลคชั่นของฉัน",
      "download_new_whi_button": "ดาวน์โหลดปุ่ม Heart ใหม่",
      "tap_the_image": "แตะที่รูปภาพที่คุณต้องการกด Heart",
      "move_your_mouse_over": "เลื่อนเมาส์ของคุณไปที่รูปภาพที่คุณต้องการกด Heart",
      "alert_no_heart": "เว็บไซต์นี้ไม่อนุญาตให้คุณกด Heart ไปยัง \"We Heart It\" กรุณาติดต่อไปยังเจ้าของเว็บไซต์เพื่อสอบถามข้อมูล ขอบคุณสำหรับการเยี่ยมชม!",
      "alert_no_pin": "เว็บไซต์นี้ไม่อนุญาตให้ทำการ Bookmark ไปยังเว็บไซต์อย่าง \"We Heart It\" กรุณาติดต่อเจ้าของเว็บไซต์เพื่อสอบถามข้อมูล ขอบคุณสำหรับการเยี่ยมชม!",
      "alert_we_heart_it": "คุณกำลังพยายามกดปุ่ม Heart ที่ weheartit.com แต่ที่เว็บไซด์ของเรา คุณเพียงแค่กดเครื่องหมาย Heart บนรูปภาพก็ได้",
      "need_help_q": "ต้องการความช่วยเหลือหรือไม่?",
      "alert_private": "คุณกำลังพยายามเอารูปภาพจากหน้าเว็บที่มีเพียงคุณที่สามารถเข้าดูได้ กรุณาเปิดลิงค์ถาวรบนหน้าต่างอื่นและ กด Heart จากที่นั่น",
      "alert_public": "รูปภาพที่%{website}มักจะเป็นภาพส่วนบุคคล",
      "alert_whi_posting_public": "การโพสต์บน We Heart It จะทำให้รูปภาพนี้กลายเป็นของสาธารณะและชื่อของคนที่โพสต์ที่%{website}อาจปรากฏอยู่บนเว็บไซต์",
      "alert_confirm": "ฉันได้อ่านและเข้าใจแล้ว",
      "alert_google_images": "คุณไม่สามารถกด Heart ที่รูปภาพใน Google Search กรุณาเปิดลิงค์รูปภาพและกด Heart ที่นั่น",
      "alert_blocked": "เว็บไซต์นี้ถูกระงับโดย weheartit.com หากคุณต้องการอุทธรณ์ กรุณาดำเนินการ %{appeal_link}",
      "appeal_here": "ที่นี่",
      "blocked": "ถูกระงับ",
      "before_you_heart": "ก่อนที่คุณจะ Heart....",
      "hey": "สวัสดี!",
      "new_release_notice": "เราเพิ่งเปิดตัวปุ่ม Heart ใหม่ของเราที่สามารถทำงานร่วมกับเบราว์เซอร์ของคุณได้อย่างเต็มที่ ดังนัั้นจึงน่าเชื่อถือมากขึ้นและยังทำงานกับเว็บไซต์ต่างๆ ที่คุณเข้าเยี่ยมชมและรักได้อย่างดี",
      "please_upgrade": "กรุณาอัพเกรดและหมั่นกด Heart ต่อไป!",
      "learn_more_and_upgrade": "เรียนรู้เพิ่มเติมและอัพเกรด",
      "new_button_available": "ปุ่ม Heart ใหม่ใช้งานได้แล้ว!",
      "close_button": "ปิด",
      "title": "หัวข้อ",
      "add_title": "เพิ่มหัวข้อของรูปภาพ",
      "tags": "แท็ก",
      "separate_with_commas": "คั่นด้วยคอมมา",
      "close": "ปิด",
      "click_to_heart": "คลิ๊กที่รูปภาพเพื่อกด Heart"
    },
    "extension": {
      "save_image_to_my_collection": "บันทึกรูปภาพไปยังคอลเลคชั่นของฉัน",
      "form": {
        "title": "ชื่อภาพ",
        "add_title_to_this_image": "เพิ่มชื่อรูปภาพนี้",
        "add_tags": "เพิ่มแท็ก",
        "tags_describe_image": "แท็กคือกลุ่มคำที่อธิบายรูปภาพ",
        "tags_example": "เช่น รูปถ่าย สีขาวและสีดำ เด็กผู้หญิง"
      },
      "sets": {
        "no_set": "ไม่มีกลุ่มภาพ",
        "new_set": "กลุ่มภาพใหม่",
        "save_to_set": "บันทึกไปยังกลุ่มภาพ",
        "new_set_name": "ชื่อกลุ่มภาพใหม่",
        "must_log_in_to_save_to_set": "คุณต้องล็อกอินที่ We Heart It เพื่อบันทึกกลุ่มภาพ"
      },
      "drop": {
        "drag_the_image_here": "ดึงรูปภาพมาที่นี่เพื่อบันทึกไปยังคอลเลคชั่นของคุณ"
      }
    },
    "extension_specific": {
      "chrome": {
        "extension_name": "We Heart It",
        "extension_description": "ส่วนเพิ่มของ We Heart It ใน Google Chrome อย่างเป็นทางการ",
        "action_default_title": "กด Heart ที่รูปภาพในหน้านี้!"
      },
      "safari": {
        "name": "We Heart It",
        "author": "Super Basic, LLC.",
        "description": "บันทึกรูปภาพโปรดของคุณในบัญชี weheartit ของคุณ "
      },
      "firefox": {
        "name": "ปุ่ม Heart",
        "description": "ปุ่ม Heart จาก We Heart It (Super Basic, LLC.)",
        "creator": "We Heart It (Super Basic, LLC.)"
      }
    }
  },
  "tr": {
    "bookmarklet": {
      "no_set": "-Grup yok-",
      "new_set": "-Yeni grup-",
      "new_set_name": "Yeni grup adı",
      "must_log_in_to_save_to_set": "Bu resmi bir gruba kaydet etmek için We Heart It'e giriş yapmış olman gerekiyor. ",
      "no_image_we_can_heart": "Bu sayfada kalp yapabileceğin herhangi bir resim bulamadık.",
      "why_is_that_q": "Neden böyle?",
      "you_can_only_heart_on_websites": "Sadece websitelerindeki resimlere kalp yapabilirsiniz,",
      "not_directly": "İnternet sağlayıcısında bulunan resim değil ",
      "go_back_to_site_and_heart": "Lütfen websitesine geri dönün ve oradan kalp yapın. ",
      "save_image_to_my_collection": "Bu resmi kolleksiyonuma ekle",
      "download_new_whi_button": "Yeni Kalp Tuşunu indirin. ",
      "tap_the_image": "Kalp yapmak istediğin resmin üstüne tıkla.",
      "move_your_mouse_over": "Kalp yapmak istediğin resmin üzerine fareni getir.",
      "alert_no_heart": "Bu websitesi We Heart It üzerinden kalp yapmaya izin vermiyor. Herhangi bir sorun varsa lütfen websitesi sahibi ile iletişime geç. Ziyaretin için teşekkürler!",
      "alert_no_pin": "Bu websitesi We Heart It gibi websitelerine yer imi olarak saklamana izin vermiyor. Herhangi bir sorun varsa lütfen websitesi sahibi ile iletişime geç. Ziyaretin için teşekkürler!",
      "alert_we_heart_it": "Weheartit.com'da Kalp Tuşunu kullanmaya çalışıyorsun, fakat sitemizde kalp yapman için resmin üstündeki kalp sembolüne tıklaman yeterli. ",
      "need_help_q": "bu konuda yardıma ihtiyacın var mı?",
      "alert_private": "Sadece senin giriş yapma iznin olduğu bir siteden resim almaya çalışıyorsun. Lütfen sabit linki başka bir pencerede aç ve oradan kalp yap.",
      "alert_public": "%{website}'daki resimler çoğunlukla özel ve gizlidir. ",
      "alert_whi_posting_public": "Bu resmi We Heart It üzerinde paylaşırsan, resim herkese açık hale gelir ve %{website}'ta bu resmi paylaşmış olan kişinin ismi sitede görülebilir. ",
      "alert_confirm": "Okudum ve anladım ",
      "alert_google_images": "Google Arama sonuç sayfasındaki resimlere kalp yapamazsın. Lütfen önce resmin linkini açıp, oradan kalp yap. ",
      "alert_blocked": "Bu site weheartit.com tarafından bloke edilmiştir. Eğer buna itiraz etmek istiyorsan, lütfen, %{appeal_link} ",
      "appeal_here": "iletişim sayfamıza git",
      "blocked": "Bloke edildi",
      "before_you_heart": "Kalp yapmadan önce...",
      "hey": "Selam!",
      "new_release_notice": "Yeni Kalp Tuşu'muzu aktif hale getirdik. Bu versiyonumuz, internet sağlayıcın ile daha iyi entegre olmuş durumdadır, ve daha sağlıklı bir paylaşım imkanı sağlar. Ziyaret ettiğin ve sevdiğin tüm websitelerinde kullanabilirsin. ",
      "please_upgrade": "Lütfen güncelle ve kalp yapmaya devam et!",
      "learn_more_and_upgrade": "Daha fazla bilgi edin ve güncelle",
      "new_button_available": "Yeni Kalp Tuşu hazır!",
      "close_button": "kapat",
      "title": "başlık",
      "add_title": "bu resme bir başlık ekle",
      "tags": "etiketler",
      "separate_with_commas": "virgüller ile ayır",
      "close": "Kapat",
      "click_to_heart": "kalp yapmak için resme tıkla"
    },
    "extension": {
      "save_image_to_my_collection": "Bu resmi kolleksiyonuma kaydet",
      "form": {
        "title": "Başlık",
        "add_title_to_this_image": "Bu resme bir başlık ekle",
        "add_tags": "Etiket ekle",
        "tags_describe_image": "Etiketler bir resmi tanımlayan sözcüklerdir",
        "tags_example": "Örnek: Fotoğrafçılık, Siyah ve Beyaz, Kız"
      },
      "sets": {
        "no_set": "-Grup yok-",
        "new_set": "-Yeni grup-",
        "save_to_set": "Şu gruba kaydet",
        "new_set_name": "Yeni grup adı",
        "must_log_in_to_save_to_set": "Bu resmi bir gruba kaydet etmek için We Heart It'e giriş yapmış olman gerekiyor. "
      },
      "drop": {
        "drag_the_image_here": "Resmi kolleksiyonuna eklemek için bu alana sürükle"
      }
    },
    "extension_specific": {
      "chrome": {
        "extension_name": "We Heart It",
        "extension_description": "We Heart It'in yasal Google Chrome uzantısı",
        "action_default_title": "Bu sayfadaki bir resme kalp yap!"
      },
      "safari": {
        "name": "We Heart It",
        "author": "Super Basic, LLC.",
        "description": "Favori resimlerini weheartit hesabına kaydet."
      },
      "firefox": {
        "name": "Kalp Tuşu",
        "description": "We Heart It'ten (Super Basic, LLC.) Kalp Tuşu",
        "creator": "We Heart It (Super Basic, LLC.)"
      }
    }
  },
  "zh-cn": {
    "bookmarklet": {
      "no_set": "- 无图片组 - ",
      "new_set": "-新增图片组-",
      "new_set_name": "新增图片组名称",
      "must_log_in_to_save_to_set": "请先登录 We Heart It，然后保存到图片组。",
      "no_image_we_can_heart": "我们无法在此页面上找到可收藏的图片。",
      "why_is_that_q": "什么原因呢？",
      "you_can_only_heart_on_websites": "只可以从网站收藏图片，",
      "not_directly": "不是浏览器里直接出现的图片。",
      "go_back_to_site_and_heart": "请返回网站，再收藏图片。",
      "save_image_to_my_collection": "把图片保存到我的图片库",
      "download_new_whi_button": "下载“心形”按钮",
      "tap_the_image": "点击想要收藏的图片。",
      "move_your_mouse_over": "选择想要收藏的图片。",
      "alert_no_heart": "该网站不允许其图片被收藏到We Heart It。如果你有什么问题，请联系网站所有者。谢谢浏览！",
      "alert_no_pin": "该网站不允许被添加到We Heart It类的网站书签中。如果你有什么问题，请联系网站所有者。谢谢浏览！",
      "alert_we_heart_it": "你正尝试使用weheartit.com上的心形按钮，不过，在我们的网站上，只可以点击图片上的心形按钮。",
      "need_help_q": "需要帮助吗？",
      "alert_private": "你正尝试从一个只有你自己才能访问的页面获取图片。请在另一个窗口打开一个永久链接，然后从那里收藏图片。",
      "alert_public": "%{website}上的图片通常是不公开的。",
      "alert_whi_posting_public": "将此图片发布到We Heart It将会使它公布于众，%{website}上的发布者可能出现在本站上。",
      "alert_confirm": "我已经阅读并了解",
      "alert_google_images": "无法从Google搜索里收藏图片。请打开图片链接，然后从那里收藏图片。",
      "alert_blocked": "此网站已经被weheartit.com阻止。如果你想求助，请点击%{appeal_link}。",
      "appeal_here": "这里",
      "blocked": "被阻止",
      "before_you_heart": "收藏之前，",
      "hey": "嘿！",
      "new_release_notice": "我们刚刚发布了新版心形按钮。作为扩展程序，它能够与你的浏览器更好地结合，因此，它更加可靠，能够与你浏览和喜欢的所有网站兼容。",
      "please_upgrade": "请先升级，然后继续收藏！",
      "learn_more_and_upgrade": "了解更多，并升级",
      "new_button_available": "新版心形按钮发布啦！",
      "close_button": "关闭",
      "title": "标题",
      "add_title": "为此图片添加标题",
      "tags": "标签",
      "separate_with_commas": "用逗号隔开",
      "close": "关闭",
      "click_to_heart": "点击图片即可收藏"
    },
    "extension": {
      "save_image_to_my_collection": "把图片保存到我的图片库",
      "form": {
        "title": "标题",
        "add_title_to_this_image": "为此图片添加标题",
        "add_tags": "添加标签",
        "tags_describe_image": "标签是指描述图片的词语。",
        "tags_example": "例如：摄影、黑白、女孩"
      },
      "sets": {
        "no_set": "- 无图片组 - ",
        "new_set": "-新增图片组-",
        "save_to_set": "保存到图片组",
        "new_set_name": "新增图片组名称",
        "must_log_in_to_save_to_set": "你需先登录 We Heart It，然后保存到图片组。"
      },
      "drop": {
        "drag_the_image_here": "将此图片拖曳到这里，以便保存到你的图片库中"
      }
    },
    "extension_specific": {
      "chrome": {
        "extension_name": "We Heart It",
        "extension_description": "We Heart It 的官方谷歌浏览器扩展程序",
        "action_default_title": "收藏此页面上的图片！"
      },
      "safari": {
        "name": "We Heart It",
        "author": "Super Basic, LLC.",
        "description": "把喜欢的图片保存到你的weheartit账户。"
      },
      "firefox": {
        "name": "心形按钮",
        "description": "We Heart It （Super Basic, LLC.）的心形按钮",
        "creator": "We Heart It （Super Basic, LLC.​）"
      }
    }
  },
  "zh-tw": {
    "bookmarklet": {
      "no_set": "-無圖片組-",
      "new_set": "-新增圖片組-",
      "new_set_name": "新增圖片組名稱",
      "must_log_in_to_save_to_set": "請先登錄 We Heart It，然後保存到圖片組。",
      "no_image_we_can_heart": "我們無法在此頁面找到可收藏的圖片。",
      "why_is_that_q": "什麽原因呢？",
      "you_can_only_heart_on_websites": "只可以從網站收藏圖片，",
      "not_directly": "而非瀏覽器上直接出現的圖片。",
      "go_back_to_site_and_heart": "請返回網站，然後收藏圖片。",
      "save_image_to_my_collection": "將圖片保存到我的圖片庫",
      "download_new_whi_button": "下載新版心形按鈕",
      "tap_the_image": "點擊想要收藏的圖片。",
      "move_your_mouse_over": "選擇想要收藏的圖片。",
      "alert_no_heart": "此網站不允許其圖片被收藏到We Heart It。如果你有什麽問題，請聯繫網站所有者。謝謝瀏覽！",
      "alert_no_pin": "此網站不允許被加入到We Heart It類的網站中。如果你有什麽問題，請聯繫網站所有者。謝謝瀏覽！",
      "alert_we_heart_it": "你正在weheartit上嘗試使用心形按鈕，不過，在我們的網站上，只可以點擊圖片上的心形按鈕。",
      "need_help_q": "需要幫助嗎？",
      "alert_private": "你正在嘗試從一個只有你才能訪問的頁面獲取圖片。請在另一個窗口打開一個永久鏈接，然後從那裡收藏圖片。",
      "alert_public": "%{website}上的圖片通常是不公開的。",
      "alert_whi_posting_public": "把此圖片發佈到We Heart It將會使它公佈於眾，%{website}的發佈者將會出現在本站上。",
      "alert_confirm": "我已經閱讀并瞭解",
      "alert_google_images": "無法從Google搜索里收藏圖片，請打開圖片鏈接，然後從那裡收藏圖片。",
      "alert_blocked": "此網站已經被 weheartit.com 阻止。如果你想求助，請點擊%{appeal_link}。",
      "appeal_here": "這裡",
      "blocked": "被阻止",
      "before_you_heart": "收藏之前，",
      "hey": "嘿！",
      "new_release_notice": "我們已經發佈了新版心形按鈕。作為擴展程序，它能夠與你的瀏覽器更好地結合，因此，它更加可靠，能夠與你瀏覽和喜愛的所有網站集成。",
      "please_upgrade": "請先升級，然後繼續收藏！",
      "learn_more_and_upgrade": "瞭解更多，并升級",
      "new_button_available": "新版心形按鈕發佈啦！",
      "close_button": "關閉",
      "title": "標題",
      "add_title": "為此圖片添加標題",
      "tags": "標籤",
      "separate_with_commas": "用逗號隔開",
      "close": "關閉",
      "click_to_heart": "點擊圖片即可收藏"
    },
    "extension": {
      "save_image_to_my_collection": "將圖片添加到我的圖片庫",
      "form": {
        "title": "標題",
        "add_title_to_this_image": "為此圖片添加標題",
        "add_tags": "添加標籤",
        "tags_describe_image": "標籤是指描述圖片的詞語",
        "tags_example": "例如：攝影、黑白、女孩"
      },
      "sets": {
        "no_set": "-無圖片組-",
        "new_set": "-新增圖片組-",
        "save_to_set": "保存到圖片組",
        "new_set_name": "新增圖片組名稱",
        "must_log_in_to_save_to_set": "請先登錄 We Heart It，然後保存到圖片組。"
      },
      "drop": {
        "drag_the_image_here": "將此圖片拖曳到這裡，以便保存到你的圖片庫"
      }
    },
    "extension_specific": {
      "chrome": {
        "extension_name": "We Heart It",
        "extension_description": "We Heart It 的官方穀歌瀏覽器擴展程序",
        "action_default_title": "將圖片保存到此頁面！"
      },
      "safari": {
        "name": "We Heart It",
        "author": "Super Basic, LLC.",
        "description": "把喜歡的圖片收藏到你的weheartit帳戶。"
      },
      "firefox": {
        "name": "心形按鈕",
        "description": "We Heart It （Super Basic, LLC.）的心形按鈕",
        "creator": "We Heart It （Super Basic, LLC.​）"
      }
    }
  }
};

whi.helper = (function () {

    // Javascript API polyfills
    if (![].forEach) {
        Array.prototype.forEach = function (callback, thisArg) {
            var l, i;
            for (l = this.length, i = 0; i < l; i++) {
                callback(this[i], i);
            }
        };
    }

    if (![].indexOf) {
        Array.prototype.indexOf = function (item) {
            var l, i;
            for (l = this.length, i = 0; i < l; i++) {
                if (this[i] === item) { return i; }
            }
            return -1;
        };
    }

    if (![].filter) {
        Array.prototype.filter = function (valid) {
            var l, i, a = [];
            for (l = this.length, i = 0; i < l; i++) {
                var item = this[i];
                if (valid(item)) { a.push(item); }
            }
            return a;
        };
    }

    if (!Object.create) {
        Object.create = function (o) {
            if (arguments.length > 1) {
                throw new Error('Object.create implementation only accepts the first parameter.');
            }
            function F() {}
            F.prototype = o;
            return new F();
        };
    }

    // selector function
    var h = function (doc, elem, attr, content) { //find/create dom elements
        doc = doc || document;
        if (/^#/.exec(elem)) { //getById
            return doc.getElementById(/[\w-]+/.exec(elem)[0]);
        } else if (/^\^/.exec(elem)) { //getByTagName
            return doc.getElementsByTagName(/\w+/.exec(elem)[0]);
        } else { //create -- elem => element tag, attr => obj with attributes, innerHTML => innerHTML content
            var e = doc.createElement(elem), prop;
            for (prop in attr) {
                if (attr.hasOwnProperty(prop)) {
                    var value = attr[prop];
                    prop = (prop === 'class' && !h.hasClassAttribute) ? 'className' : prop;
                    e.setAttribute(prop, value);
                }
            }
            if (content !== undefined) {
                e.innerHTML = content;
            }
            return e;
        }
    };

    // DOM cross browser functions
    h.addEventListener = (function () {
        if (document.addEventListener) {
            return function (object) {
                object && object.addEventListener.apply(object, [].slice.call(arguments, 1));
            }
        } else {
            return function (object) {
                object && object.attachEvent('on' + arguments[1], arguments[2]);
            }
        }
    }());
    h.removeEventListener = (function () {
        if (document.removeEventListener) {
            return function (object) { object && object.removeEventListener.apply(object, [].slice.call(arguments, 1)); }
        } else {
            return function (object) { object && object.detachEvent('on' + arguments[1], arguments[2]); }
        }
    }());
    h.stopPropagation = function (event) {
        event = event || window.event;
        event.clancelBubble = true;
        if (event.stopPropagation) { event.stopPropagation(); }
    };
    h.getElementsByClassName = (function () {
        if (document.getElementsByClassName) {
            return function (doc) { return doc && doc.getElementsByClassName.apply(doc, [].slice.call(arguments, 1)); }
        } else {
            return function getElementsByClassName(node, classname) {
                var a = [];
                var re = new RegExp('(^| )' + classname + '( |$)');
                var els = node.getElementsByTagName("*");
                for(var i=0, j=els.length; i<j; i++)
                if (re.test(els[i].className)) { a.push(els[i]); }
                return a;
            }
        }
    }());
    h.getComputedStyle = (function () {
        if (!window.getComputedStyle) {
            return function (el, pseudo) {
                var style = {};
                style.getPropertyValue = function(prop) {
                    var re = /(\-([a-z]){1})/g;
                    if (prop == 'float') { prop = 'styleFloat'; }
                    if (re.test(prop)) {
                        prop = prop.replace(re, function () { return arguments[2].toUpperCase(); });
                    };
                    var currentStyle = el.currentStyle || el.style;
                    return currentStyle[prop] === 'medium' ? '0px' : currentStyle[prop];
                };
                return style;
            };
        } else {
            return function () { return getComputedStyle.apply(window, arguments); };
        }
    }());
    h.hasClassAttribute = (function () {
        var e = document.createElement('div');
        e.setAttribute('class', 'test');
        return e.className === 'test';
    }());
    // dom position
    h.findPosX = function (element) {
        var doc = element.ownerDocument;
        var border = 0;
        var domParams = h.getDOMParams(element);
        var scrollLeft = domParams.isFixed ? 0 : (doc.body.scrollLeft || doc.documentElement.scrollLeft);
        border = border + parseFloat(h.getComputedStyle(element, '').getPropertyValue('border-left-width'));
        border = border + parseFloat(h.getComputedStyle(element, '').getPropertyValue('padding-left'));
        return border + element.getBoundingClientRect().left + scrollLeft;
    };
    h.findPosY = function (element) {
        var doc = element.ownerDocument;
        var border = 0;
        var domParams = h.getDOMParams(element);
        var scrollTop = domParams.isFixed ? 0 : (doc.body.scrollTop || doc.documentElement.scrollTop);
        border = border + parseFloat(h.getComputedStyle(element, '').getPropertyValue('border-top-width'));
        border = border + parseFloat(h.getComputedStyle(element, '').getPropertyValue('padding-top'));
        return border + element.getBoundingClientRect().top + scrollTop;
    };
    // callback function should return next DOM node to process or falsy value to break
    h.traverseDOM = function (currentNode, callback) {
        var lastNode = currentNode;
        while (currentNode = callback(currentNode)) { lastNode = currentNode; }
        return lastNode;
    };
    h.body = function (doc) { return doc.body || doc.getElementsByTagName('body')[0]; };
    h.head = function (doc) { return doc.head || doc.getElementsByTagName('head')[0]; };
    h.title = function (doc) {
        var headTitleElement = doc.getElementsByTagName('title')[0];
        var title = doc.title && doc.title.replace(/(^\s+|\s+$)/, '');
        title = title || (headTitleElement && headTitleElement.text && headTitleElement.text.replace(/(^\s+|\s+$)/, ''));
        title = title || "Untitled";
        return title.substring(0, 200);
    };
    h.isAncestor = function (element, ancestor) {
        if (element === ancestor) return false;
        var isA = false;
        h.traverseDOM(element, function (currentElement) {
            isA = currentElement === ancestor;
            return !isA && currentElement.parentNode;
        });
        return isA;
    };
    h.getDOMParams = function (element) {
        if (element.isFixed && element.zIndex) {
            return { isFixed: element.isFixed(), zIndex: element.zIndex() }
        }

        var objectIndex = 0;
        var isFixed = false;
        var fixedParent;
        h.traverseDOM(element, function (currentNode) {
            isFixed = isFixed || h.getComputedStyle(currentNode, '').getPropertyValue('position') === 'fixed' || currentNode.style.position === 'fixed';
            if (isFixed && !(currentNode instanceof HTMLBodyElement) && !(currentNode instanceof HTMLHtmlElement)) { fixedParent = currentNode; };
            var zIndex = ''+h.getComputedStyle(currentNode, '').getPropertyValue('z-index') || ''+currentNode.style.zIndex;
            objectIndex += !zIndex.match(/^(auto|0)$/) ? +zIndex : 1;
            var isDocument = currentNode.parentNode === currentNode.ownerDocument
            return isDocument ? undefined : currentNode.parentNode;
        });
        return { zIndex: objectIndex, isFixed: isFixed, fixedParent: fixedParent };
    };
    h.DOM2Array = function (list) {
        var array = [];
        for (var i = 0, l = list.length; i < l; i++) {
            array[i] = list[i];
        }
        return array;
    };
    h.isBookmarklet = false;
    h.docs = function (rootDoc) {
        var docs = [];
        h.DOM2Array(rootDoc.getElementsByTagName('iframe')).forEach(function (iframe) {
            var a = rootDoc.createElement('a');
            a.href = iframe.src;
            try {
                if ((a.hostname === location.hostname) || (a.hostname === "")) {
                    var iframeDocument = (iframe.contentDocument || iframe.contentWindow.document);
                    if (iframeDocument.location.href) {
                        docs.push(iframeDocument);
                    }
                }
            } catch (e) {}
        });
        return docs;
    };
    h.findParent = function (element, attributeName, matcher) {
        while (element.parentNode) {
            if (element.getAttribute(attributeName) && element.getAttribute(attributeName).match(matcher)) return element;
            element = element.parentNode;
        }
    };
    // recursively calls callback for all documents & subdocuments
    h.withDocsTree = function (rootDoc, callback) {
        var docs = h.docs(rootDoc);

        docs.forEach(function (doc) {
            h.withDocsTree(doc, callback);
        });
        callback(rootDoc);
    };
    h.IOSMessage = function (doc, message) {
        var iframe = doc.createElement("IFRAME");
        iframe.setAttribute("src", "js-frame:" + message);
        doc.documentElement.appendChild(iframe);
        iframe.parentNode.removeChild(iframe);
    };
    h.androidMessage = function (message, params) {
        if (!window.javaInterface) return;
        window.javaInterface.onLog("whi.helper.androidMessage(" + message + ")");
        window.javaInterface[message](params);
    };
    h.ajax = function (method, url, isAsync) {
        var req = new XMLHttpRequest();
        req.open(method, url, isAsync);
        req.send(null);
        return req;
    };
    h.log = function () {
      console.log(Array.prototype.slice.call(arguments));
      if (!window.javaInterface) return;
      var args = [].splice.call(arguments,0);
      window.javaInterface.onLog(args.join(' '));
    };
    return h;

}());

whi.AlertHtml = function (doc) {
    this.document = doc;
};

whi.AlertHtml.prototype.alertData = {
    image: whi.i18n.t('bookmarklet.you_can_only_heart_on_websites') + ' ' +whi.i18n.t('bookmarklet.not_directly') + '\n\n' + whi.i18n.t('bookmarklet.go_back_to_site_and_heart'),
    blocked: '<div class="whi-title">' + whi.i18n.t('bookmarklet.blocked') + '</div><div class="whi-content">' + whi.i18n.t('bookmarklet.alert_blocked', { appeal_link: "<a href=\"" + whi.website + "/contact?subject=" + "Blocked+website+appeal" + "&blocked_host=" + location.hostname + "\">" + whi.i18n.t('bookmarklet.appeal_here') + "</a>" }) + "</div>",
    noHeart: '<div class="whi-title">' + whi.i18n.t('http://weheartit.com/bookmarklet.hey') + '</div><div class="whi-content">' + whi.i18n.t('bookmarklet.alert_no_heart') + "</div>",
    noPin: '<div class="whi-title">' + whi.i18n.t('http://weheartit.com/bookmarklet.hey') + '</div><div class="whi-content">' + whi.i18n.t('bookmarklet.alert_no_pin') + "</div>",
    weHeartIt: '<div class="whi-title">' + whi.i18n.t('http://weheartit.com/bookmarklet.hey') + '</div><div class="whi-content">' + whi.i18n.t('bookmarklet.alert_we_heart_it') + " <a href='http://help.weheartit.com/customer/portal/articles/59356-hearting-images-on-whi' target='_blank'><br><br>" + whi.i18n.t('bookmarklet.need_help_q') + "</div>",
    private: '<div class="whi-title">' + whi.i18n.t('http://weheartit.com/bookmarklet.hey') + '</div><div class="whi-content">' + whi.i18n.t('bookmarklet.alert_private') + " <a href='http://help.weheartit.com/customer/portal/articles/59360-hearting-from-private-pages' target='_blank'><br><br>" + whi.i18n.t('bookmarklet.need_help_q') + "</a></div>",
    googleImages: '<div class="whi-title">' + whi.i18n.t('http://weheartit.com/bookmarklet.hey') + '</div><div class="whi-content">' + whi.i18n.t('bookmarklet.alert_google_images') + "</span>",
    public_facebook: '<div class="whi-title">' + whi.i18n.t('bookmarklet.before_you_heart') + '</div><div class="whi-content">' + whi.i18n.t('bookmarklet.alert_public', { website: "Facebook" }) + '<br><br>' + whi.i18n.t('bookmarklet.alert_whi_posting_public', { website: "Facebook" }) + '</div><button id="whi-confirm" class="whi-btn big">' + whi.i18n.t('bookmarklet.alert_confirm') + '</button></div>',
    public_vk: '<div class="whi-title">' + whi.i18n.t('bookmarklet.before_you_heart') + '</div><div class="whi-content">' + whi.i18n.t('bookmarklet.alert_public', { website: "VK" }) + '<br><br>' + whi.i18n.t('bookmarklet.alert_whi_posting_public', { website: "VK" }) + '</div><button id="whi-confirm" class="whi-btn big">' + whi.i18n.t('bookmarklet.alert_confirm') + '</button></div>',
    promoteExtensions: '<div class="whi-title">' + whi.i18n.t('bookmarklet.new_button_available') + '</div><div class="whi-content"><div class="whi-alert-left"><img src="' + whi.website + '/images/heart-button/preview_chrome_mac.png' + '"></img></div>' + whi.i18n.t('bookmarklet.new_release_notice') + "<br /><br />" + whi.i18n.t('bookmarklet.please_upgrade') + "<br /><br />" + "<a href='" + whi.website + "/heart-button' target='_blank' class='whiButtonBig'>" + whi.i18n.t('bookmarklet.learn_more_and_upgrade') + '</a></div>',
    noImages: '<div class="whi-title">' + whi.i18n.t('http://weheartit.com/bookmarklet.hey') + '</div><div class="whi-content">' + whi.i18n.t('bookmarklet.no_image_we_can_heart') + " <a href='http://help.weheartit.com/customer/portal/articles/59402-invalid-images' target='_blank'>" + whi.i18n.t('bookmarklet.why_is_that_q') + "</a></div>",
    server_error: '<div class="whi-title">' + whi.i18n.t('bookmarklet.error') + '</div><div class="whi-content">' + whi.i18n.t('bookmarklet.alert_server_error') + "</div>"
};

whi.AlertHtml.prototype.show = function (content) {
    this.hide();

    var that = this;
    var whiWindow = whi.helper(this.document, "div", { id: "whiAlertWindow", "class": "whi-alert" }, content);
    var closeButton = whi.helper(this.document, "a", { href: "#", "class": "whi-close-button" });

    whiWindow.appendChild(closeButton);
    whi.helper.body(this.document).appendChild(whiWindow);

    setTimeout(function () {
        if (whi.bookmarklet.browserDetector().isTouch()) {
            whiWindowText.style.padding = '0';
            var topPosition = window.innerHeight/2 - whiWindow.clientHeight/2;
            whiWindow.style.marginTop = '0';
            whiWindow.style.top = ((topPosition > 30 ? topPosition : 30) + pageYOffset) + 'px';
            whiWindow.style.position = "absolute";
        }
    }, 30);

    var alertWidth = this.document.documentElement.clientWidth - 80;
    var maxWidth = 600;
    var width = alertWidth - maxWidth > 0 ? maxWidth : alertWidth;
    whiWindow.style.width = width + "px";
    whiWindow.style.marginLeft = -width/2 + "px";

    closeButton.onclick = function () { that.hide(); };
};

whi.AlertHtml.prototype.hide = function () {
    var whiAlert = this.document.getElementById("whiAlertWindow");
    if (whiAlert) { whiAlert.parentNode.removeChild(whiAlert); }
};

whi.ImageElement = function (image) {
    this.image = image;
    this.document = image.ownerDocument;
};

whi.ImageElement.prototype.media = function () {
    return this.image.src.replace(/\.\.\//, '');
};

whi.ImageElement.prototype.isValid = function () {
    var that = this;
    var isValid = true;
    var conditions = [];

    if(whi.helper.isBookmarklet) {
      conditions.push({ valid: (this.image.src != null && this.image.src.length > 5), msg: 'Bad url' });
    }
    else {
      conditions.push({ valid: this.width() >= 240, msg: 'Bad width' });
      conditions.push({ valid: this.height() >= 200, msg: 'Bad height' });
    }
    conditions.push({ valid: !this.image.src.match("http://weheartit.com/weheartit.com"), msg: 'From weheartit' });
    conditions.push({ valid: !(this.image.src.match("http://weheartit.com/facebook.com") && this.image.src.match('messaging')), msg: 'Facebook chat' });
    conditions.push({ valid: !this.image.src.match(/^data/), msg: 'Inline image' });
    conditions.push({ valid: this.document.location.href.match(/tumblr\.com/) ? this.image.id !== 'vignette' : true, msg: 'Tumblr vignette' });

    var isValid = true;
    conditions.forEach(function (condition) {
        //if (!condition.valid) { whi.helper.log(condition.msg + " " + that.width() + " x " + that.height() + " " + that.image.src); }
        isValid = isValid && condition.valid;
    });
    return isValid;
};

whi.ImageElement.prototype.complete = function () {
    return this.image.complete;
};

whi.ImageElement.prototype.via = function () {
    var parent = this.image.parentNode;
    var count = 0;
    while(parent != null && count < 5) {
      if(parent.tagName === "A") {
        return parent.href;
      }
      parent = parent.parentNode;
      count++;
    }
    return this.document.location.href;
};

whi.ImageElement.prototype.href = function() {
    var parent = this.image.parentNode;
    return parent.tagName === "A" ? parent.href : "";
};

whi.ImageElement.prototype.label = function() {
    return this.image.title || this.image.getAttribute('aria-label') || this.image.alt || "";
};

whi.ImageElement.prototype.caption = function() {
    var parent = this.image.parentNode;
    var caption = "";
    if (parent.tagName === "FIGURE") {
        var captionNodes = parent.getElementsByTagName('figcaption');
        caption = captionNodes[0] && captionNodes[0].innerHTML;
    }
    return caption;
};

whi.ImageElement.prototype.thumbnail = function (callback) {
    callback(this.media());
};

whi.ImageElement.prototype.width = function () {
    if (typeof this.image.naturalWidth === 'undefined') {
        return this.tmpImage().width;
    }
    return this.image.naturalWidth;
};

whi.ImageElement.prototype.height = function () {
    if (typeof this.image.naturalHeight === 'undefined') {
        return this.tmpImage().height;
    }
    return this.image.naturalHeight;
};

// for IE width/height
whi.ImageElement.prototype.tmpImage = function () {
    this._tmpImage = this._tmpImage || new Image();
    this._tmpImage.src = this.media();
    return this._tmpImage;
};

whi.ImageElement.isImageElement = function (image) {
    return true;
};

// factory
whi.ImageElement.createImageElement = function (image) {
    var types = [
        'GoogleImageElement',
        'MobileGoogleImageElement',
        'BingImageElement',
        'MobileTumblrDashboardImageElement',
        'TumblrDashboardImageElement',
        'TumblrImageElement',
        'InstagramImageElement',
        'PinterestImageElement'
    ];

    var type = types.filter(function (type) {
        return whi[type] && whi[type]['is' + type](image);
    }).shift();

    return (type && new whi[type](image)) || new whi.ImageElement(image);
};

whi.GoogleImageElement = function (image) {
    whi.ImageElement.call(this, image);
};

whi.GoogleImageElement.prototype = Object.create(whi.ImageElement.prototype);

whi.GoogleImageElement.prototype.url = function () {
    return this.image.parentElement.href.match(/imgrefurl=(.+?)&/)[1];
};

whi.GoogleImageElement.prototype.media = function () {
    var originalMedia = this.image.parentElement.href.match(/imgurl=(.+?)&/)[1];
    return originalMedia.replace(/\.\.\//, '');
};

whi.GoogleImageElement.prototype.width = function () {
    return this.image.parentElement.href.match(/w=(.+?)&/)[1];
};

whi.GoogleImageElement.prototype.height = function () {
    return this.image.parentElement.href.match(/h=(.+?)&/)[1];
};

whi.GoogleImageElement.prototype.thumbnail = function (callback) {
    var img = this.image.src || (this.image.dataset && this.image.dataset.src) || this.image.getAttribute('data-src');
    callback(img);
};

whi.GoogleImageElement.isGoogleImageElement = function (image) {
    return image.ownerDocument.location.href.match(/google.com\/search/) && image.className.match(/rg_i/) && !image.parentNode.className.match(/rg_di/) && image.parentElement.getAttribute('href') !== null;
};

whi.BingImageElement = function (image) {
    whi.ImageElement.call(this, image);
};

whi.BingImageElement.prototype = Object.create(whi.ImageElement.prototype);

whi.BingImageElement.prototype.media = function () {
    var originalMedia = this.image.parentElement.getAttribute('m').match(/oi:"(.+?)"/)[1];
    return originalMedia.replace(/\.\.\//, '');
};

whi.BingImageElement.prototype.via = function () {
    return "http://" + this.image.parentElement.getAttribute('t3');
};

whi.BingImageElement.prototype.width = function () {
    return +this.image.parentElement.getAttribute('t2').match(/^(\d+?)\s/)[1];
};

whi.BingImageElement.prototype.height = function () {
    return +this.image.parentElement.getAttribute('t2').match(/x (\d+?) /)[1];
};

whi.BingImageElement.prototype.thumbnail = function (callback) {
    var img = this.image.src || this.media();
    callback(img);
};

whi.BingImageElement.isBingImageElement = function (image) {
    return !!/MSIE/.exec(navigator.userAgent) && image.ownerDocument.location.href.match(/bing.com\/images/) && image.parentElement.className === "dv_i" && !image.previousElementSibling;
};

whi.TumblrDashboardImageElement = function (image) {
    whi.ImageElement.call(this, image);
    whi.helper.log('creating TumblrDashboardImageElement');
    this._media = image.src;
};

whi.TumblrDashboardImageElement.prototype = Object.create(whi.ImageElement.prototype);

whi.TumblrDashboardImageElement.prototype.media = function () {
    return this._media;
};

whi.TumblrDashboardImageElement.prototype.isValid = function () {
    var isLightbox = whi.helper.findParent(this.image, 'id', /tumblr_lightbox/);
    return !isLightbox && whi.ImageElement.prototype.isValid.call(this)
};

whi.TumblrDashboardImageElement.prototype.via = function () {
    var url = null;
    var postElement = whi.helper.findParent(this.image, 'data-post-id', /^\d+$/);
    if (postElement) {
        link = this.document.getElementById('permalink_' + postElement.dataset.postId)
        url = link && link.href;
    }
    return url || location.href;
};

// STATIC
whi.TumblrDashboardImageElement.isTumblrDashboardImageElement = function (image) {
    var isMobile = !!whi.helper.findParent(image, 'className', /mh_post/);
    return image.ownerDocument.location.href.match(/tumblr\.com\/dashboard/) && !isMobile;
};

whi.TumblrImageElement = function (image) {
    whi.ImageElement.call(this, image);
    whi.helper.log('creating TumblrImageElement');
};

whi.TumblrImageElement.prototype = Object.create(whi.ImageElement.prototype);

whi.TumblrImageElement.prototype.via = function () {
    var match;
    var url;
    var currentUrl = this.document.location.href;

    var post = whi.helper.traverseDOM(this.image, function (currentElement) {
        return (!currentElement.ownerDocument || currentElement.className.match(/(^|\s)post($|\s)/)) ? null : currentElement.parentNode;
    });

    if (post && post !== this.document) {
        url = post.getElementsByClassName('username')[0] && post.getElementsByClassName('username')[0].firstChild.href;
        url = url || (post.getElementsByClassName('post_permalink')[0] && post.getElementsByClassName('post_permalink')[0].href); // tumblr.com.dashboard on iPad
        url = url || (post.getElementsByClassName('permalink')[0] && post.getElementsByClassName('permalink')[0].href); // tumblr.com.dashboard on iPad
    } else if (currentUrl.match(/tumblr.com\/reblog/)) {
        url = this.document.querySelector('[name=redirect_to]').value;
        if (url.match(/redirect_to=(.+)$/)) {
            var match = this.document.querySelector('[name=redirect_to]').value.match(/redirect_to=(.+)/);
            url = match && unescape(match[1]);
        } else if (url.match(/dashboard/)) {
            match = this.document.getElementById('post_two').innerHTML.match(/href="(.+?)"/);
            url = match && match[1];
        }
    } else if (this.document.location.host.match(/.+\.tumblr.com/) && this.document.location.host !== 'www.tumblr.com') {
        url = this._viaFromJSON();
    }

    url = url || currentUrl;
    var link = document.createElement("a");
    link.href = url;
    if (!(link.protocol && link.hostname)) {
        url = currentUrl;
    }
    return url;
};

whi.TumblrImageElement.prototype._viaFromJSON = function () {
    if (!whi.TumblrImageElement._data) {
        var req = whi.helper.ajax('GET', '/api/read/json?num=50', false);
        if (req.status === 200) {
            whi.TumblrImageElement._data = JSON.parse(/(\{.+\});/.exec(req.responseText)[1]).posts;
        }
    }
    return whi.TumblrImageElement._data && this._findUrl(whi.TumblrImageElement._data, this.image);
};

whi.TumblrImageElement.prototype._findUrl = function (posts) {
    var i;
    var property;
    posts = posts.filter(function (post) { return post.type === "photo"; });
    for (i=0; i<posts.length; i++) {
        for (property in posts[i]) {
            if (posts[i].hasOwnProperty(property) && property.match(/^photo-url/) && posts[i][property] === this.image.src) {
                return posts[i].url;
            }
        }
    }
};

// STATIC
whi.TumblrImageElement.isTumblrImageElement = function (image) {
    return image.ownerDocument.location.host.match(/tumblr\.com/);
};

whi.TumblrImageElement.load = function (callback) {
    var that = this;
    var req = whi.helper.ajax('GET', '/api/read/json?num=50', true);
    req.onreadystatechange = function () {
        if (req.readyState === 4 && req.status === 200) {
            whi.TumblrImageElement._data = JSON.parse(/(\{.+\});/.exec(req.responseText)[1]).posts;
            callback();
        }
    };
};

whi.InstagramImageElement = function (image) {
    whi.ImageElement.call(this, image);
};

whi.InstagramImageElement.prototype = Object.create(whi.ImageElement.prototype);

whi.InstagramImageElement.prototype.media = function () {
    return this.image.getAttribute('src');
};

whi.ImageElement.prototype.via = function () {
    return this.document.location.href;
};

whi.InstagramImageElement.isInstagramImageElement = function (image) {
    return image.className.match('Image');
}

whi.PinterestImageElement = function (image) {
  image.setAttribute('src', image.src.replace(/pinimg.com\/(.+?)\//, "pinimg.com/736x/"));
  image.setAttribute('style', "");
  whi.ImageElement.call(this, image);

  // save original sizes for later
  this.originalHeight = this.super_height();
  this.originalWidth = this.super_width();

};

whi.PinterestImageElement.prototype = Object.create(whi.ImageElement.prototype);

whi.PinterestImageElement.isPinterestImageElement = function (image) {
    return image.ownerDocument.location.href.match(/pinterest\.com/) && image.className.match(/pinImg/)
}

whi.PinterestImageElement.prototype.super_height = whi.ImageElement.prototype.height;
whi.PinterestImageElement.prototype.super_width = whi.ImageElement.prototype.width;

whi.PinterestImageElement.prototype.height = function () {
  return this.originalHeight;
};

whi.PinterestImageElement.prototype.width = function () {
  return this.originalWidth;
};


whi.VideoElement = function (video) {
    this.document = video.ownerDocument;
    this.video = video;
};

whi.VideoElement.prototype.media = function () {
    return this.video.src;
};

whi.VideoElement.prototype.via = function () {
    return this.document.location.href;
};

whi.VideoElement.prototype.width = function () {
    return this.video.clientWidth;
};

whi.VideoElement.prototype.height = function () {
    return this.video.clientHeight;
};

whi.VideoElement.prototype.href = function () {
    var parent = this.video.parentNode;
    return parent.tagName === "A" ? parent.href : undefined;
};

whi.VideoElement.prototype.label = function () {
    return this.video.title || this.video.getAttribute('aria-label') || this.video.alt || undefined;
};

whi.VideoElement.prototype.caption = function () { return; };

whi.VideoElement.prototype.thumbnail = function (callback) {
    var videoId = "";
    if (this.media().match(/vimeo\.com/)) {
        videoId = this.media().match(/video\/(\d+)?/)[1];
        whi.bookmarklet.jsonp(this.document, "http://vimeo.com/api/v2/video/" + videoId + ".json", "whi.bookmarklet.vimeo_" + videoId + "_cb");
        whi.bookmarklet['vimeo_' + videoId + "_cb"] = function (response) {
            var data;
            var thumbnail;
            if (response.success) {
                try { thumbnail = JSON.parse(response.data)[0].thumbnail_medium } catch (ex) {}
            }
            callback(thumbnail)
        };
    } else if (this.media().match(/youtube/)) {
        videoId = this.media().match(/embed\/(.+)\?/) && this.media().match(/embed\/(.+)\?/)[1];
        videoId = videoId || (this.media().match(/\/v\/(.+?)\?/)[1] && this.media().match(/\/v\/(.+?)\?/)[1]);
        callback("http://img.youtube.com/vi/" + videoId + "/mqdefault.jpg");
    }
};

whi.IFrameVideoElement = function (video) {
    var currentHref = video.getAttribute("src"),
    connector = /\?/.exec(currentHref) ? "&" : "?";
    video.setAttribute("src", currentHref + connector + "wmode=transparent");

    whi.VideoElement.call(this, video);
};

whi.IFrameVideoElement.prototype = Object.create(whi.VideoElement.prototype);

whi.bookmarklet = (function () {
    "use strict"
    var $h = whi.helper;

    var module = {
        settings: {
            bookmarkletVersion: "http://weheartit.com/2.1.10",
            heartingMethod: "bookmarklet"
        },

        init: function (isRunningFromBookmarklet, preferences) {
            whi.website = whi.website || "http://weheartit.com/";

            this.userInfo = preferences;
            var that = this;
            var alertContent;
            var page = this.page;

            module.alertHtml = new whi.AlertHtml(document);

            page.addCss(document);
            var errorCode = page.validate();
            var errorDesc = page.getValidateErrorDesc(errorCode)
            if (typeof errorCode !== 'undefined') {
                alertContent = module.alertHtml.alertData[errorDesc];
                page.isHtmlAlert(errorCode) ? module.alertHtml.show(alertContent) : alert(alertContent);
                page.validationActions[errorDesc] && page.validationActions[errorDesc](isRunningFromBookmarklet);
            } else {
                if (page.isVideo()) {
                    window.location = whi.website + "/create_entry/?media=" + encodeURIComponent(page.url()) + "&title=" + encodeURIComponent($h.title(document)) + "&title_original=" + encodeURIComponent($h.title(document)) + "&via=" + encodeURIComponent(page.url()) + "&bookmarklet_version=" + module.Setting.bookmarkletVersion + "&extension_version=" + module.Setting.extensionVersion + "&hearting_method=" + module.Setting.heartingMethod;
                } else {
                    var allElements = module.parser.getElements();

                    if (isRunningFromBookmarklet && typeof preferences.dismissedPromotedExtensions !== 'undefined' && !preferences.dismissedPromotedExtensions) {
                        alertContent = module.alertHtml.alertData.promoteExtensions;
                        module.alertHtml.show(alertContent);
                    }

                    if (!allElements.images.length && !allElements.images.length) {
                        module.alertHtml.show(module.alertHtml.alertData.noImages);
                    } else {
                        this.grid.render(allElements);
                    }
                }
            }
        },

        browserDetector: function (theNavigator) {
            var _navigator = theNavigator || navigator;
            return {
                isInternetExplorer: function () {
                    return !!/MSIE/.exec(_navigator.userAgent);
                },
                isFirefox: function () {
                    return !!/Firefox\//.exec(_navigator.userAgent);
                },
                isChrome: function () {
                    return !!/Chrome\//.exec(_navigator.userAgent);
                },
                isSafari: function () {
                    return !!/Version\/.+Safari\//.exec(_navigator.userAgent);
                },
                isSupportedSafari: function () {
                    if (!this.isSafari()) {
                        return false;
                    }
                    var versionFound = /Version\/(\d+(\.\d+)?)/.exec(_navigator.userAgent);
                    if (!versionFound) {
                        return false;
                    }
                    var version = parseFloat(versionFound[1]);
                    return version >= 5.1 && !_navigator.userAgent.match(/mobile/i);
                },
                isTouch: function () {
                    return 'ontouchstart' in window;
                },
                isIPhone: function () {
                    return (/iPhone/i).test(_navigator.userAgent);
                }
            };
        },

        parser: {
            getData: function () {
                var elements = this.getElements();
                var format = function (element) {
                    return {
                        src: decodeURIComponent(element.media()),
                        width: element.width(),
                        height: element.height(),
                        via: decodeURIComponent(element.via())
                    }
                };

                var imagesData = elements.images.map(format);
                var videosData = elements.videos.map(format);
                var jsonData = JSON.stringify({ "title": $h.title(document), "images": imagesData, "videos": videosData });
                $h.androidMessage('onGetData', jsonData);
                return jsonData;
            },
            getElements: function (onDocumentParsed) {
                var that = this;
                var elements;
                var allElements = { images: [], videos: [] };

                $h.withDocsTree(document, function (doc) {
                    elements = that._parseDocument(doc)

                    allElements.images = allElements.images.concat(elements.images);
                    allElements.videos = allElements.videos.concat(elements.videos);

                    onDocumentParsed && onDocumentParsed(doc, elements)
                });
                return allElements;
            },
            _parseDocument: function (doc) {
                var model = module.model(doc);
                var images = model.image.all().filter(function (item) { return item.isValid(); });
                var videos = model.video.all();
                return { images: images, videos: videos }
            }
            // TODO: restore waitForLoad
        },

        model: function (doc) {
            return {
                image: {
                    all: function () {
                        var tmpImages, images;

                        images = $h.DOM2Array(doc.images);
                        if (doc.location.href.match(/m\.facebook/)) {
                            tmpImages =  $h.DOM2Array($h.getElementsByClassName(doc, 'img'));
                            tmpImages.forEach(function (image) {
                                image.src = image.style.backgroundImage.match(/url\((.+)\)/) && image.style.backgroundImage.match(/url\((.+)\)/)[1];
                                if (image.src) { images.push(image); }
                            });
                        }

                        if (doc.location.href.match(/serenahphotography.com.au/)) {
                            var tmpImages = $h.DOM2Array(doc.getElementsByTagName('td'));
                            tmpImages.forEach(function (image) {
                                imagePath = image.getAttribute('background');
                                if (imagePath) {
                                    image.src = encodeURI('http://' + location.host + '/' + imagePath)
                                    images.push(image);
                                }
                            });
                        }

                        var divImages = $h.DOM2Array(doc.getElementsByTagName('div'));
                        divImages.forEach(function (image) {
                          if(image.style && image.style.backgroundImage && image.style.backgroundImage.match(/url\("(.+)"\)/)) {
                            var newImg = new Image();
                            newImg.src = image.style.backgroundImage.match(/url\("(.+)"\)/) && image.style.backgroundImage.match(/url\("(.+)"\)/)[1];
                            if (newImg.src) {
                              images.push(newImg);
                            }
                          }
                        });

                        if (doc.location.href.match(/pinterest.com/)) {
                          tmpImages =  $h.DOM2Array($h.getElementsByClassName(doc, 'pinImage'));
                          tmpImages.forEach(function (image) {
                            if(image.tagName == 'DIV') {
                              image.src = image.style.backgroundImage.match(/url\((.+)\)/) && image.style.backgroundImage.match(/url\("(.+)"\)/)[1];
                              if (image.src) {
                                var newImg = new Image();
                                newImg.src = image.src;
                                images.push(newImg);
                              }
                            }
                          });
                        }

                        if (doc.location.host === "www.tumblr.com" && doc.location.pathname === "/") {
                            tmpImages =  $h.DOM2Array($h.getElementsByClassName(doc, 'stage'));
                            tmpImages.forEach(function (image) {
                                image.src = image.style.backgroundImage.match(/url\((.+)\)/) && image.style.backgroundImage.match(/url\((.+)\)/)[1];
                                if (image.src) { images.push(image); }
                            });
                        }

                        if (doc.location.host === "instagram.com") {
                            tmpImages =  $h.DOM2Array($h.getElementsByClassName(doc, 'Image'));
                            tmpImages.forEach(function (image) {
                                image.src = image.getAttribute('src');
                                images.push(image);
                            });
                        }

                        var foundImages = images.map(function (image) {
                            return whi.ImageElement.createImageElement(image);
                        });

                        return foundImages;
                    }
                },
                video: {
                    all: function () {
                        var embeds = $h.DOM2Array($h(doc, "^embed"));
                        var iframes = $h.DOM2Array($h(doc, "^iframe"));
                        var foundVideos = [];

                        foundVideos = foundVideos.concat(embeds.filter(this._isEmbed).map(this._makeEmbed));
                        foundVideos = foundVideos.concat(iframes.filter(this._isIFrame).map(this._makeIFrame));

                        return foundVideos;
                    },
                    _isEmbed: function (embed) {
                        return embed.src.match("youtube.com|vimeo.com")
                          && !embed.src.match("youtube.com/subscribe_embed")
                    },
                    _isIFrame: function (iframe) {
                        return iframe.src.match("youtube.com|vimeo.com")
                          && !iframe.src.match("youtube.com/subscribe_embed")
                    },
                    _makeEmbed: function (embed) {
                        return new whi.VideoElement(embed);
                    },
                    _makeIFrame: function (iframe) {
                        return new whi.IFrameVideoElement(iframe);
                    }
                }
            }
        },

        page: {
            validate: function () {
                var error;
                if (this.isImage()) { error = 0; }
                else if (this.isBlocked()) { error = 1; }
                else if (this.isNoHeart()) { error = 2; }
                else if (this.isNoPin()) { error = 3; }
                else if (this.isWeHeartIt()) { error = 4; }
                else if (this.isPrivate()) { error = 5; }
                else if (this.isFacebook()) { error = 7; }
                else if (this.isVK()) { error = 9; }
                $h.androidMessage('onValidate', error);
                return error;
            },
            isImage: function () {
                var a = document.createElement('a');
                a.href = this.url();
                return $h.body(document).childNodes[0].tagName === 'IMG' && !!a.pathname.match(/\.(jpg|png|gif)$/);
            },
            isPrivate: function () {
                return this.url().match("google.com/reader");
            },
            isNoHeart: function () {
                return this.blockMetaTags().indexOf('noheart') !== -1;
            },
            isNoPin: function () {
                return this.blockMetaTags().indexOf('nopin') !== -1;
            },
            isWeHeartIt: function () {
                return document.location.hostname.match(/weheartit\.com/);
            },
            isFacebook: function () {
                return document.location.hostname.match(/facebook\.com/);
            },
            isVK: function () {
                return document.location.hostname.match(/vk\.com/);
            },
            isVideo: function () {
                return this.url().match("vimeo.com\/[0-9]+|vimeo.com\/m\/[0-9]+|youtube.com.+watch|m.youtube.com.+index");
            },
            isBlocked: function () {
                return module.userInfo.blocked;
            },
            isPromoteExtensions: function () {
                return true;
            },
            blockMetaTags: function () {
                var meta, blocked = [];
                var metas = document.getElementsByTagName('meta');
                for (var i = 0; i < metas.length; i++) {
                    meta = metas[i];
                    if (meta.name === "weheartit" && meta.content === "noheart") { blocked.push(meta.content); }
                    if (meta.name === "pinterest" && meta.content === "nopin") { blocked.push(meta.content); }
                }
                return blocked;
            },
            url: function () { return document.location.href; },
            charSet: function () {
                return document.characterSet || document.charset;
            },
            addCss: function (doc) {
                doc = doc || document;
                if (doc.getElementById('whi-css')) return;

                var urls = [];
                if ($h.isBookmarklet) {
                  urls.push(whi.website + "/stylesheets/bookmarklet.css");
                }
                else if (module.browserDetector().isChrome()) { return }
                else if (module.browserDetector().isFirefox()) { return }
                else if (module.browserDetector().isSafari()) {
                  urls.push(safari.extension.baseURI + 'css/grid.css')
                  urls.push(safari.extension.baseURI + 'css/alert.css')
                }

                urls.forEach(function (url) {
                  $h.head(doc).appendChild($h(doc, "link", { rel: "stylesheet", type: "text/css", href: url, id: "whi-css" }));
                })
            },
            getValidateErrorDesc: function (code) {
                var descriptions = {
                    0: 'image',
                    1: 'blocked',
                    2: 'noHeart',
                    3: 'noPin',
                    4: 'weHeartIt',
                    5: 'private',
                    6: 'googleImages',
                    7: 'public_facebook',
                    9: 'public_vk'
                };
                return descriptions[code];
            },
            validationActions: {
                image: function () { location.href = "http://help.weheartit.com/customer/portal/articles/59361-hearting-images-not-in-a-webpage" },
                public: function (isRunningFromBookmarklet) {
                    var button = document.getElementById('whi-confirm');
                    $h.addEventListener(button, 'click', function () {
                        var allElements = module.parser.getElements(function (doc, elements) {
                            module.page.addCss(doc);
                        });
                        whi.bookmarklet.grid.render(allElements);
                        module.alertHtml.hide();
                    });
                },
                public_facebook: function (isBookmarklet) { this.public(isBookmarklet); },
                public_vk: function (isBookmarklet) { this.public(isBookmarklet); }
            },
            isHtmlAlert: function (errorCode) {
                // array contains codes for html alerts
                return [1,2,3,4,5,6,7,9].indexOf(errorCode) >= 0;
            }
        },

        grid: {
            render: function (allElements) {
                var that = this;
                var contents = document.createElement('div');
                contents.id = "whi-contents";

                var thumbnails = document.createDocumentFragment();

                allElements.images.forEach(function (image) {
                    thumbnails.appendChild(that._createThumbnail(image));
                });

                allElements.videos.forEach(function (video) {
                    thumbnails.appendChild(that._createThumbnail(video));
                });

                var topBar = this._createTopBar();
                var overlay = document.createElement('div');
                var grid = document.createElement('span');

                overlay.className = 'whi-overlay';

                var body = document.body, html = document.documentElement;
                var height = Math.max(body.scrollHeight, body.offsetHeight, html.clientHeight, html.scrollHeight, html.offsetHeight);

                overlay.style.height = height + "px";
                this._lastScrollTop = document.body.scrollTop || document.documentElement.scrollTop;
                document.body.scrollTop = 0;
                document.documentElement.scrollTop = 0; // for Firefox

                grid.className = 'whi-grid';
                grid.appendChild(thumbnails);
                overlay.appendChild(grid);
                contents.appendChild(topBar);
                contents.appendChild(overlay);

                $h.body(document).appendChild(contents);
            },
            _createTopBar: function () {
                var that = this;

                var bar = document.createElement('div');
                bar.innerHTML = '<span class="whi-excl whi-left">click images to heart</span><a href="https://weheartit.com/" class="logo"></a>';
                bar.className = "whi-topbar";

                var closeButton = document.createElement('a');
                closeButton.className = "whi-button-big whi-right";
                closeButton.innerText = whi.i18n.t("bookmarklet.close_button");
                closeButton.textContent = whi.i18n.t("bookmarklet.close_button");
                closeButton.href = "#";
                $h.addEventListener(closeButton, 'click', function (event) {
                    event.preventDefault();
                    that._clear();
                    document.body.scrollTop = that._lastScrollTop;
                    document.documentElement.scrollTop = that._lastScrollTop;
                });
                bar.appendChild(closeButton);

                return bar;
            },
            _createThumbnail: function (item) {
                var that = this;

                var heart = document.createElement('span');
                heart.className = "whi-heart";

                var button = document.createElement('span');
                button.className = "whi-button";
                button.appendChild(heart);

                var gridButton = document.createElement('span');
                gridButton.className = "whi-grid-button";
                gridButton.appendChild(button);

                var thumbnail = document.createElement('span');
                thumbnail.className = 'whi-grid-image';
                thumbnail.appendChild(gridButton);

                $h.addEventListener(thumbnail, 'click', function () {
                    that._showPopup(item);
                });

                item.thumbnail(function (url) {
                    thumbnail.style.backgroundImage = 'url(' + url + ')';
                });

                return thumbnail;
            },
            _showPopup: function (element) {
                var width = 700;
                var height = 415;
                var screenHeight = screen.height;
                var screenWidth = screen.width;
                var left = Math.round(screenWidth / 2 - width / 2);
                var top  = Math.round(screenHeight / 2 - height / 2);
                var website = sessionStorage.getItem('whiWebsite') || whi.website;

                var params = [];
                params.push("media=" + encodeURIComponent(element.media()));
                params.push("via=" + encodeURIComponent(element.via()));
                params.push("encoding=" + encodeURIComponent(document.characterSet));
                params.push("extension_version=" + encodeURIComponent(module.Setting.extensionVersion));
                params.push("hearting_method=" + encodeURIComponent(module.Setting.heartingMethod));
                params.push("original_title=" + $h.title(document));
                params.push("href=" + encodeURIComponent(element.href()));
                params.push("label=" + encodeURIComponent(element.label()));
                params.push("caption=" + encodeURIComponent(element.caption()));
                params.push("popup=1");
                var query = params.join('&');

                var url = website + '/heart-it/new_entry?' + query;
                window.open(url, '_blank', 'resizable=1,width=' + width + ',height=' + height + ',left=' + left + ',top=' + top);
            },
            _clear: function () {
                var element;
                (element = document.getElementById('whi-contents')) && element.parentNode.removeChild(element);
                (element = document.getElementById('whi-css')) && element.parentNode.removeChild(element);
                (element = document.getElementById('whi-js')) && element.parentNode.removeChild(element);
                $h.body(document).className = $h.body(document).className.replace('no-touch', '');
            },
            _lastScrollTop: 0
        },

        jsonp: function (doc, url, callbackName) {
            var link = doc.createElement('a');
            link.href = url;
            link.search = (link.search || "?") + "&callback=" + callbackName;

            var jsonpScriptTag = doc.createElement("script");
            jsonpScriptTag.src = link.href;
            jsonpScriptTag.type = "text/javascript";

            $h.body(doc).appendChild(jsonpScriptTag);

            var clearScriptTag = setTimeout(function () {
                clearTimeout(clearScriptTag);
                jsonpScriptTag.parentNode.removeChild(jsonpScriptTag);
            }, 1000)
        },
    };

    return module;
}());


// overwrite isBookmarkelt setting, dont try to guess it
whi.helper.isBookmarklet = true;

window.whiStart = function (preferences) {
  whi.bookmarklet.init(true, preferences);
};
whi.bookmarklet.jsonp(document, whi.website + "/current_user/info?host=" + encodeURIComponent(location.hostname), "whiStart");

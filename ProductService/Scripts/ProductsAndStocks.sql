CREATE EXTENSION IF NOT EXISTS "uuid-ossp";

CREATE TABLE "Products" (
	"Id" uuid PRIMARY KEY DEFAULT uuid_generate_v4(),
	"Title" text NOT NULL,
	"Description" text,
	"Price" integer
);

CREATE TABLE "Stocks" (
	"Id" uuid PRIMARY KEY DEFAULT uuid_generate_v4(),
	"ProductId" uuid REFERENCES "Products",
	"Count" integer,
	FOREIGN KEY ("ProductId") REFERENCES "Products" ("Id")
);

INSERT INTO "Products"("Title", "Description", "Price")
VALUES('Red Velvet Cake', 'Red velvet cake is essentially a butter cake, though it is frequently made with oil instead of butter. In addition, cocoa is added to the cake batter to create the distinct red velvet flavor — originally it was a reaction between buttermilk and the raw cocoa widely available at the time of red velvets inception that caused a ruddy-hued crumb.', 10),
('Carrot Cake', 'Carrot cake uses the leavening practices of butter cake, but instead of butter uses a neutral oil like vegetable or canola oil. For this reason, it will keep a little longer than butter cakes but can sometimes come out on the greasy side.', 12),
('Pumpkin Chiffon Cake', 'Chiffon cake is a light, airy cake for when you want dessert but don’t want to be bogged down with something heavy. Chiffon boasts both the rich taste and crumbly texture of a yellow cake as well as the lightness and sponge of an angel food cake. This pumpkin chiffon cake recipe is tender and sweet with hints of fall spices!', 15),
('Strawberry Sponge Cake', 'Many cakes—Genoise, angel, chiffon—fall into the realm of sponge cake, which is basically a cake made with egg whites, flour and sugar that classically relies on air for leavening. The most basic version doesn’t contain fat, but variations will add it for moisture. Favored for its versatility, sponge cake is tender and bouncy and will soak up the flavors of anything it’s paired with (like fruit or coffee)', 7),
('Glazed Blueberry Cake', 'Yellow cake lovers will go nuts over this glazed blueberry cake with a moist, tender crumb. It won’t weigh you down—even if you have two slices—and we’d even argue it’s perfectly acceptable to eat for breakfast. Basically a fruit salad, right?', 9),
('Angel Food Cake', 'Look, not every cake needs multiple layers and heaps of buttercream frosting. That is the beauty of our angel food cake recipe. It is light and airy and satisfyingly simple. The timing could not be better, either, since slices are best served with plenty of fresh berries and whipped cream. Consider it your go-to summer dessert.', 11),
('Devil’S Fook Cake', 'Behold, the counterpart to angel food cake. Devil’s food is a sinfully rich (heh) chocolate cake that can be flavored with either unsweetened baking chocolate or cocoa powder. It’s often made with boiling water as the main liquid instead of milk, and the chocolate level is intense.', 10),
('Citrus Shortcake', 'Shortcake is closer in style to an American biscuit than a cake, and though some folks think the name refers to its low height, it’s actually a reference to the method of baking (in old English, to “short” something meant to crisp it up with the addition of fat). In any case, the dessert is lightly sweet, crumbly, buttery and a natural pair for fresh fruit and whipped cream.', 14),
('Chocolate Glazed Espresso Cheesecake', 'Everyone needs a little pick-me-up. Sometimes it’s a latte. Sometimes it’s chocolate. But what about a recipe with a little bit of both? That’s the thinking behind our espresso cheesecake. We bake it New York-style—extra tall and rich—and make it even more impressive by coating it in shiny chocolate glaze.', 15),
('Flourless Chocolate Cake', 'We know what you’re thinking: How can anyone make a cake without flour? And why would you? In the case of flourless chocolate cake (the most common kind), the batter is an aerated, egg-based custard that’s baked into a dense, sliceable dessert. It’s incredible fudgy and naturally gluten-free—what’s not to love?', 12)

INSERT INTO "Stocks"("ProductId", "Count")
VALUES ('39705279-e3eb-4480-9549-2bef8f2868c7', 3),
('62b0b960-12d6-47d8-b5f4-76f9bd71b5d2', 4),
('e9b55f73-6e00-427a-9b96-e2c3ee1444ee', 1),
('76c528c8-9437-488b-bfa5-cd641b0baa4c', 10),
('a39aea50-761e-416c-847d-ee7dfc1a57c8', 4),
('7dacb9dd-ad4c-4fbc-a408-45b96146d4a3', 7),
('9b938897-3b1c-434c-8ef7-74b319fdf723', 3),
('22c68126-f774-48d2-a593-fbe3f2a6e129', 6),
('38393988-cbf6-462f-8ef8-96a99697de78', 5),
('09a38990-86da-428e-9c55-cb79321bd34b', 3)

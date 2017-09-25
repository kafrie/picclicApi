#Ember getting started

##Ember Training and Learnings

###Important links:
* This page is summary of video - http://campus.codeschool.com/courses/try-ember/level/1/section/1/video/1
* http://emberjs.com/
* Link to Woodlands project - https://github.com/kafrie/woodland

###Installing Ember:
1. Install https://nodejs.org/en/download/stable/ for windows
2. Install https://git-scm.com/downloads 
3. Make sure to select to use the git path, if it doesnt give you the option go set the path yourself by using
	C:\Users\{user}\AppData\Local\GitHub\PortableGit_{guid}\cmd
4. Run `npm install -g ember-cli` in your command prompt (This will install the ember framework)

###Creating New Ember ProjectL
1. In your command prompt run `path>ember new {project name}`
![image](https://cloud.githubusercontent.com/assets/17876815/13810352/b1a5e670-eb6f-11e5-86ff-3f18bd7fcc69.png)
2. Via command prompt go to the project - `path>cd  projectname`
3. Run `path\project>ember serve`
  This will start up a development server with live reload.
  
  ![image](https://cloud.githubusercontent.com/assets/17876815/13811555/ceb81aec-eb76-11e5-87ee-34d72ae912f3.png)
	
####Possible issue:
_Failed to execute "git ls-remote --tags --heads git://github.com/ember-cli/ember-cli-shims.git", exit code of #128
fatal: unable to connect to github.com_ 
* To solve this run:
	`git config --global url."https://".insteadOf git://`

Even after doing this you might get (or still get):
fatal: remote error:
  You can't push to git://github.com/kafrie/picclicApi.git
  Use https://github.com/kafrie/picclicApi.git
  
You would think the above mentioned use https instead of git would work, in some cases it does not work, but what worked for me after I did above is following:
` git remote set-url origin git@github.com:myusername/repository.git`

A must read, especially if you get weird errors after ember serve, such as `Uncaught Error: Attempting to inject an unknown injection`
- http://emberigniter.com/update-latest-ember-data-cli/

###Restoring an Ember project from Git clone
On occasion you might want to clone your project to a seperate machine and continue development from there (e.g. Sepperation between Office and Home).

Because gitignore will exclude node_modules and the bower_components, `ember serve` will throw an error complaining about modules being missing or empty.

To get your project back to a working state run the following on `path\projectName>`:

1. npm install
2. npm install -g bower
3. bower install
	- Sometimes you will get `bower ENOGIT        git is not installed or not in the PATH`, but in your mind you know you set your path. as per https://hassantariqblog.wordpress.com/2015/11/06/bower-enogit-git-is-not-installed-or-not-in-the-path/ . If you do run into this issue in Node run the following `set PATH=%PATH%;C:\Program Files\Git\bin;` and then bower install. All should be fine now.

Now when running `ember serve` everything should be back in order.

###First Overview of App

####Templates
Template will tell Ember what html to render for each page.

`app/templates/application.hbs` is the default template, if index.hbs is added with something in it, the `{{outlet}}` will display what is in there (default empty).

Creating another template will not automatically display. For this we need to tell ember in the router.js file where to go.

To link to a different landing page for a user:
`{{#link-to 'landingPage'}}landingPage{{/link-to}}` 

####Router
Manages the application state, will map the state into the url where user is and need to go to.

app/router.js

```javascript

Router.map(function() {
    ---Application endpoints mapped here---
});

Router.map(function() {
    this.route('orders', {path: '/orders'});
});

```

___OR___

```javascript

Router.map(function() {
    this.route('orders');
});

```

(path will be inferred)

![image](https://cloud.githubusercontent.com/assets/17876815/13815628/a570b224-eb8b-11e5-8cf7-23d70b6b9fc8.png)

(based on template created orders.hbs)

####Routes
Collecting and organising data and handing off the data to templates to be rendered.

Ember CLI provides generator for creating a route and updating the router.

`ember generate route <route-name>`

![image](https://cloud.githubusercontent.com/assets/17876815/13903700/eda41498-ee86-11e5-81cf-f6f3c2a96a09.png)

As you can see it will automatically generate:
* file in routes for you (newroute.js)
* template file (newroute.hbs)

This will also add the route `this.route('newroute');` in the router.js

___Routes extended___

The js file in the routes folder `newroute.js` will extend the ember route and can be customised for this 
particular route without effecting any other route.

_Customizing the Route_

To customise the route you can add a function or hook, in this case a model function will be added which can return anything
to be available to the template.

In my case I added a model hook with returning an array to the template:

app\routes\newroute.js - 

```javascript

export default Ember.Route.extend({
    model() {
        return [{ id:'myid', name:'myname' },
        { id:'myid2', name:'myname2' }
        ];
    }
});

```

and in the app\templates\newroute.hbs - 

```javascript
New route <br>

{{#each model as |name|}}
    id - {{name.id}}<br>
    name - {{name.name}}<br>
{{/each}}

```

This will give me the following outcome:

![image](https://cloud.githubusercontent.com/assets/17876815/13903882/e001aed0-ee8c-11e5-88a5-c0abf025a1de.png)

___Define Dynamic Segments in Router___

First we need to map where the dynamic segement will route. In the router map section we add: `this.route('newroutes', {path: '/newroute/:newroute_id'});`.
The `:` in the path represents the dynamic segment and where it starts, these dynamic segments is placeholders for when you pass your 'id' or other params.
For instance if I have a request coming in for /newroute/1, the browser will go through mapping to try and find a match.

```javascript

Router.map(function() {
  this.route('orders');
  this.route('newroute');
  this.route('newroutes', {path: '/newroute/:newroute_id'}); `-finds match here for /newroute/1 and will then navigate to newroutes template`
});

``` 

Once it macthes the router will pickup on the dynamic value and capture that portion - in this case ':newroute_id' matcheds the '1'.
It will then map the 1 to a newroute_id variable and put it into a hash {newroute_id:1}, which is then sent to the route to tell it what the user request is.

Whatever dynamic data is found will be handed over to the route and can be passed into the model hook to find the data the user has requested. This is usually represented by `params`as can be seen in code snippet bewow:

```javascript

export default Ember.Route.extend({
    model(params) {
        return [{ id:'1', name:'myname' },
        { id:'2', name:'myname2' }
        ].findBy('id', params.newroute_id);
    }
});

```
___NOTE___ in te snippet above we also find a specific record by Id and say that we want this id to be what ever was passed through
 as params, in this case newroute_id was mapped as 1. This will return the record with the id of 1.
 
 * Take note of the following
 
    * Each template will need its own routes js file containing the data that need to be manipulated. So for our template newroutes.hbs we need in our app/routes
     another js file called newroutes which will have the code as mentioned above. In the case of this example we have a newroute.js and a newroutes.js file, both which contains the model hook. The only difference is,
     because my newroutes.hbs only need to display information we requested, the model in the js file in routes will contain params and a findBy.
     
```javascript

`newroute.js`
export default Ember.Route.extend({
    model() {
        return [{ id:'1', name:'myname' },
        { id:'2', name:'myname2' }
        ];
    }
});

`newroutes.js`
export default Ember.Route.extend({
    model(params) {
        return [{ id:'1', name:'myname' },
        { id:'2', name:'myname2' }
        ].findBy('id', params.newroute_id);
    }
});

```

After the data is manipulated to represent what the user has requested, we need a template that can display this data to the user. In this case a simple newroutes.hbs template is created to display what was requested:

```html

this is from new route <br>
id = {{model.id}}
name = {{model.name}}

```   

Outcome after typing newroute/1:

![image](https://cloud.githubusercontent.com/assets/17876815/13919672/309dff78-ef6f-11e5-9ce3-5ee4a42645e4.png)

We can add a link to for each name or id for our data displayed to the user in our template file for newroute. This will allow the user to click on displayed data and be routed to data that they want to see instead of
typing the path to navigate to.

```html

{{#each model as |name|}}
    {{#link-to "newroutes" name}} `Note that next to link-to there are extra paramaters - "newroutes"=route name and name=object to view`
        id - {{name.id}}
    {{/link-to}}
        ::: name - {{name.name}}<br>    
{{/each}}

```

___Nested Route___

Because we do not want to lose the list that will navigate us to the users requested information (we want this list of names on the same page in order to navigate around).

As the current templates are sibling of one another they will end up replacing one another as we navigate and fill the `{{outcome}}` on the main page. This means that when we navigate to newroutes it will replace newroute and
we loose the list of items from our page. In oreder to not loose this listing we use nested routes, which will allow us to display multiple templates on the same page.

```javascript

`nested route
Add anonymous function in the parent route which will make the newroutes a child route of the newroute parents route. This will automaticaaly inherit the newroute as parent and you do not need to 
specify the path: /newroute/:newroute_id `

Router.map(function() {
  this.route('orders');
  this.route('newroute', function(){
      this.route('newroutes', {path: '/:newroute_id'});
  });
});

``` 

To make the nesting work correctly, a few more changes are needed. From here we go to the `#each` in our newroute template and change it to:

```html

`because newroutes is now a child of newroute we need to change the link to route name to be newroute.newroutes`
{{#each model as |name|}}
    {{#link-to "newroute.newroutes" name}}
        id - {{name.id}}
    {{/link-to}}
        ::: name - {{name.name}}<br>    
{{/each}}

{{outlet}}

```

Outcome

![image](https://cloud.githubusercontent.com/assets/17876815/13922421/78c7ace2-ef7c-11e5-9356-c3298802c623.png)


###Models and Services
To centralise the data we will create services. Services are long living objects available throughout the app (singleton). For this the data will be stored in a data repository service.

`ember gemerate service <service-name>`

![image](https://cloud.githubusercontent.com/assets/17876815/13931309/03eb0172-efa3-11e5-82fb-8e524b6356d5.png)

Services are made available within other objects by using `Ember.inject.service()`

```javascript

`app\service\store.js`
export default Ember.Service.extend({
    getOrders(){
    `The data was moved over from the newroute model hook into the getOrders function`
       return [{ id:'1', name:'myname' },
        { id:'2', name:'myname2' }
        ];
    }
});

`app\routes\newroute.js`
export default Ember.Route.extend({
    model() {
    	const store = this.get('store');
        return store.getOrders();
     },
    
    `store: equals local name of service and ('store') equals the name of the service to inject`
    store: Ember.inject.service('store')
    `because the service name matches the property name you can leave or drop the service name and ember will infer it from property 	     name e.g. store: Ember.inject.service()`
});

```

After injection, the store service becomes available as the "store" property.

To finalise the code and make sure the data is used correctly everywhere, where it is needed a few more changes are needed. Final code snippet below:

```javascript

`app/services/store`
export default Ember.Service.extend({
    getOrdersById(id){
        const orders = this.getOrders();
        return orders.findBy('id',id); `the find by was moved from newroutes.js`
    },
    
    getOrders(){
       return [{ id:'1', name:'myname' },
        { id:'2', name:'myname2' }
        ];
    }
});

`app/routes/newroute/newroutes.js`
export default Ember.Route.extend({
    model(params) {
        const id = params.newroute_id;
        const store = this.get('store');
        return store.getOrdersById(id);
    },
    
    store: Ember.inject.service()
});

```

####Models
Models represent the underlying (sometimes persisted), data of the application.

```javascript

`Defines the product model as subclass of Ember.Object`
import Ember from 'ember'

export default Ember.Object.extend({});

```

Ember.Object gives:
1. Consistent interface for creating and destroying record
2. Object lifecycle events and hooks
3. Properties and property observation functionality. How templates are updates when data changes.

New models need to be imported into the store to be used. One way of doing this is through using relative paths 

```javascript

import Ember from 'ember';
import Order from '../models/order';
import Products from '../models/products'
import Line-Items from '../models/line-items'
 
export default Ember.Service.extend({
    getOrdersById(id){/*...*/},
    getOrders(){/*...*/}
});

```
However if the store ever moves this will break the app. A better way would be to use absolute or project paths. This is done with the app name in the import statement

```javascript

import Ember from 'ember';
import Order from 'woodlands/models/order';
import Products from 'woodlands/models/products'
import Line-Items from 'woodlands/models/line-items'
 
export default Ember.Service.extend({
    getOrdersById(id){/*...*/},
    getOrders(){/*...*/}
});

```

Creating some data to use throuhout the app:

```javascript

`app/services/store.js`
import Ember from 'ember';
import Order from '../models/order';
import Products from '../models/products';
import LineItems from '../models/line-items';

`private function not available to rest of system`
const products = [
    Products.create({title: 'Tent', price:10}),
    Products.create({title: 'Sleeping', price:11}),
    Products.create({title: 'Flashlight', price:12}),
];

const orders = [
    Order.create({
        id: '123', name: 'Piet',
        items:[
            LineItems.create({product: products[0], quantity: 1}),
            LineItems.create({product: products[1], quantity: 1}),
            LineItems.create({product: products[2], quantity: 0})
        ]    
    }),
    Order.create({
        id: '124', name: 'Koos',
        items:[
            LineItems.create({product: products[0], quantity: 0}),
            LineItems.create({product: products[1], quantity: 2}),
            LineItems.create({product: products[2], quantity: 0})
        ]       
    })
];
 
export default Ember.Service.extend({
    getOrdersById(id){
        return orders.findBy('id',id);
    },    
    getOrders(){ return orders; },    
    getProducts (){ return products; } `this will make the private function available throughout the system when called`
});

```

To make all of this data connecting together nicely. We need to change our template a bit to display the data for the specific customer we click on. In the app/templates/newroute/newroutes.hbs we change the template file to reflect the following

```html

this is from new route <br><br><br>
id = {{model.id}}<br>
name = {{model.name}}<br>

`Above is what we had, now we want to display the product and the quantity for each customer.`
{{#each model.items as |item|}}

product = {{item.product.title}} ....
quantity = {{item.quantity}}<br>

{{/each}}

```

The outcome will be as follows:

![image](https://cloud.githubusercontent.com/assets/17876815/13985841/4d50c268-f0ff-11e5-94e8-e7157d28a2f4.png)

To manage the orders we need an empty order object to work with. To do this head back to the store and create a new empty order function to work with.

```javascript

export default Ember.Service.extend({
    getOrdersById(id){ return orders.findBy('id',id); },    
    getOrders(){ return orders; },    
    getProducts (){ return products; },
    
    /* To manage new orders information need an empty order record to work with 
    newOrder() returns a new order record with one lineitem record per product*/
    newOrder(){
        return Order.create({
            items: products.map(
                (product)=>{
                    return LineItems.create({
                        product: product
                    });
                })
        });
    }
});

```

The app\templates\newroute\newroutes.hbs need to be updated in order for these orders and order form to be displayed.

```html	

this is from new route <br><br><br>
id = {{model.id}}<br>
name = {{model.name}}<br>


<form>
    <label for="name">Name</label>
    <!-- Binding input fields is done through ember input handlebars 
    the value property is then used to bind the value you want to bind
    value=model.name
    Ember provides {{input}} helpers, which keeps bound properties and input fields in sync-->
    {{input type="text" id="name" value=model.name}}<br>
        <label>
            {{#each model.items as |item|}}
                {{item.product.title}}
                {{input type="number" min="0" value=item.quantity}}<br> <!--no quotes around the property gives helper direct access to the property for maipulation -->
            {{/each}}
        </label>
     <input type="submit" id="newroutes">       
</form>

```

###Actions

What we need to do now is to hook up the submit button to have it listen for the submit event coming in. In Ember this is done through Actions.

Actions map generic DOM events to specific application activities and functions

![image](https://cloud.githubusercontent.com/assets/17876815/13999916/f9b53180-f13e-11e5-8aeb-81db75e6e6d7.png)

Actions are mapped in templates using the {{action}} helper, defined on element to watch.

```javascript

{{action "actionname" on="event"}}

<form {{action "createOrder" /*fires the create order action*/
	model /*any extra parameters are passed to the triggered action as parameter*/
	on="submit" /*action triggers when form emits submit event*/
	}}>
/**/
     <button type="submit" id="newroutes">Order</button>       
</form>

```

Action handlers are functions defined in an actions block on the route or its parent

After defining the `createOrder` function as action, we need to create the `createOrder` functio. 

```javascript
/* app/routes/newroute/newroutes.js*/

export default Ember.Route.extend({
/*create an actions block*/
    actions: {
    /*createOrder matches the name of function we mentioned in the template
    As seen in template we passed 'model' as a paramter therefore we pass createOrder a parameter of order */
        createOrder(order){
            const name = order.get('name');
            alert(name + ' order saved');      
        }        
    },
    model(params) {/*....*/ },
    store: Ember.inject.service()
});

```

Outcome after hitting submit button:

![image](https://cloud.githubusercontent.com/assets/17876815/14012801/895b03fa-f1a8-11e5-8091-6a28f4101cec.png)

Preferably we would want different action than an alert, we would want to navigate to a different page. Routes have a `transitionTo`
function used to navigate to other routes.

```javascript

/*app/routes/newroute/newroutes.js*/
export default Ember.Route.extend({
    actions: {
        createOrder(order){
            /*take order we are given and hand it over to the store
            then instruct ember that I want to transition user from this route to another*/
            this.get('store').saveOrder(order);
            /*'newroute.newroutes' name of route want to transition to 
            order = optional model parameter, in this case the model I would like to show*/
            this.transitionTo('newroute.newroutes', order);
        }        
    },
    model(params) {/*...*/ },
    
    store: Ember.inject.service()
});

```

Now we need to save the orders in the store. New orders are saved by giving them an ID and adding them to the collection

Now we go back to the app/services/store.js and create the `saveOrder` that was used above.

```javascript

export default Ember.Service.extend({
    /*...*/
    saveOrder(order){
        order.set('id', '9898'); /*setting mock id for example, usually created by db*/
        orders.pushObject(order); /*pushObject similar to js push, but in addition triggers value changed events
        Note and be careful, in this case the object Orders and the paramater passed is order. You want to append to the object check 	spelling and small errors like appending to model instead of data object*/
    }
});

```

Outcome of this will be the new order added to the bottom of the list of orders that already exists

![image](https://cloud.githubusercontent.com/assets/17876815/14016849/0ae001d6-f1c3-11e5-845e-e505f0b0c92d.png)

###Computed properties

Computed properties  are function-calculated, cached properties. Ember ships with ~30 predefined computed property macros.

Computed properties provide:

* property value computed from a function call
* automatic invalidation when dependent properties change

We want to introduce computed properties as currently in the items ordered list we break the [law of Demeter](https://en.wikipedia.org/wiki/Law_of_Demeter)

`{{item.product.title}}` we do not want to access title of product through item (or another object), we want it from its direct friend.

To do this we go back to the `app/models/line-items.js`

```javascript

export default Ember.Object.extend({
 title: Ember.computed('product.title', /*dependent properties of the function are listed, when property changes it is recalculated */
 function(){ /*function return value cached as the lineitems title property*/
     return this.get('product.title');
 })
});

`__OR__`

export default Ember.Object.extend({
 title: Ember.computed.alias('product.title'),
});

```

You can now go to the template and change the following and everything should still be the same as before

```html

<form>
        <label>
            {{#each model.items as |item|}}
               <!--{{item.product.title}}-->
                Item = {{item.title}}::
                Quantity = {{item.quantity}}::
                <!--{{item.product.price}}-->
                Price = {{item.unitPrice}}::
                Cost = ''::
                Cost% = ''::<br>
            {{/each}}
        </label>
        Total = ''   
</form>

```

To calculate the cost of the amount of units ordered we will add the following to the code

```javascript

`app\models\line-items.js`
import Ember from 'ember';

export default Ember.Object.extend({
/*parseInt() used because quantity is being set from form input which are string values.
adding the base 10 indicator is good practise*/
    price: Ember.computed('quantity', 'unitPrice', function(){
        return parseInt(this.get('quantity'), 10) * this.get('unitPrice');
    }),
 title: Ember.computed.alias('product.title'),
 unitPrice: Ember.computed.alias('product.price')
});

`app\models\order.js`
import Ember from 'ember';

export default Ember.Object.extend({
    /*items = name of collection to use
    price = element property to map
    itemprices = array of lineitem prices
    mapped array will automatically update if the items array changes or its price values change*/
    itemPrices: Ember.computed.mapBy('items', 'price'),
    /*price = contain order price
    itemPrices = collection to sum */
    price: Ember.computed.sum('itemPrices')
});

`app\templates\newroute\newroutes.hbs`
`this` is from `new` route <br><br><br>
id = {{model.id}}<br>
name = {{model.name}}<br>


<form>
        <label>
            {{#each model.items as |item|}}
                Item = {{item.title}}::
                Quantity = {{item.quantity}}::
                Price = ${{item.unitPrice}}::
                Cost = ${{item.price}}::
                Cost% = ''::<br>
            {{/each}}
        </label>
         Total = ${{model.price}}  
</form>
```

Outcome

![image](https://cloud.githubusercontent.com/assets/17876815/14019603/eacb761c-f1d3-11e5-9122-4b7a100763c9.png)

###Properties and Components

The percentages need to be implemented and styled based on values (above 50% bold).

The lineitem cost percentage has a dynamic presentation and is isolated.

1. presentation changes to bold when above 50%
2. Value is not used anywhere else in the system, only on order receipt page
3. if user click on percentage they can see the formula for calculation

For these reasons one will use components. Components are reusable way to combine a template with action handling behaviour. Components are best used for creating common pieces of an application that will be used time and again.

Ember cli provides a generator for creating a component and its template.

![image](https://cloud.githubusercontent.com/assets/17876815/14020255/a1da0f6a-f1d6-11e5-9055-ec718503e7f8.png)

![image](https://cloud.githubusercontent.com/assets/17876815/14020347/072652fc-f1d7-11e5-90d3-fe8a8ac39c95.png)

__Note__ Components must have at least one hyphen in their name, this conforms with the custom element spec.

Components have their own properties, functions and behaviour.

```javascript

`app\components\item-percentage.js`
import Ember from 'ember';

export default Ember.Component.extend({
    /*gte = ember macro meaning greater than, returns true if value given is greater or equal to value property*/
    isImportant: Ember.computed.gte('percentage', 50),
    /*itemPrice and orderPrice values will be provided from calling the template */
    percentage: Ember.computed('itemPrice', 'orderPrice', function(){
        return this.get('itemPrice') / this.get('orderPrice') * 100;
    })    
});

`app\templates\components\item-percentage.hbs`
/*Use handlebars {{if}} helper to dynamically set or unset an important class*/
<span class="{{if isImportant 'important'}}">  
    /*The span gets an important class if component isimportant value returns true*/ 
    {{percentage}}%    
</span>

<style type="text/css">
    .important {
        color: blue;
        font-weight: bold;
    }
</style>

`app\templates\newroute\newroutes.hbs`
form>
        <label>
            {{#each model.items as |item|}}
                Item = {{item.title}}::
                Quantity = {{item.quantity}}::
                Price = ${{item.unitPrice}}::
                Cost = ${{item.price}}::
                /*item-percentage = This renders the item percentage component into this location
                itemPrice and orderPirce is passed in
                */
                Cost% = {{item-percentage itemPrice=item.price orderPrice=model.price}}::<br>
            {{/each}}
        </label>
        Total = ${{model.price}}   
</form>

```

